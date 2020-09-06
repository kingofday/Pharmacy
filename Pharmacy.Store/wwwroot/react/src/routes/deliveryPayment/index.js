import React from 'react';
import { Spinner, Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import Skeleton from '@material-ui/lab/Skeleton';

import strings from './../../shared/constant';
import srvDelivery from './../../service/srvDelivery';
import { commaThousondSeperator } from './../../shared/utils';
import { ShowInitErrorAction, HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import deliveryCostImage from './../../assets/images/delivery-cost.svg';
import { toast } from 'react-toastify';
import Button from './../../shared/Button';

class DeliveryPayment extends React.Component {
    state = {
        loading: true,
        uniqueId: 0,
        type: '',
        price: 0,
        btnInProgresss: false,
        gatewayUrl: '',
    };
    _orderId = null;
    _isMounted = true;

    async componentDidMount() {
        const { params } = this.props.match;
        this._orderId = params.id;
        this.props.hideInitError();
        await this._fetchData();
    }

    async _fetchData() {
        this.setState(p => ({ ...p, loading: true }));
        let apiRep = await srvDelivery.getPrice(this._orderId);
        this.setState(p => ({ ...p, loading: false }));
        if (!this._isMounted) return;
        if (!apiRep.success) {
            this.props.showInitError(this._fetchData.bind(this), apiRep.message);
            return;
        }
        this.setState(p => ({ ...p, ...apiRep.result }));
    }

    async componentWillUnmount() {
        this._isMounted = false;
    }

    async _pay() {
        this.setState(p => ({ ...p, btnInProgresss: true }));
        let submit = await srvDelivery.payPrice(this._orderId);
        this.setState(p => ({ ...p, btnInProgresss: false }));
        if (submit.success)
            window.open(submit.result, '_self');

        else toast(submit.message, { type: toast.TYPE.ERROR });

    }

    _goToBasket() {
        this.setState(p => ({ ...p, redirect: '/basket' }));
    }

    _continue() {
        this.changedProductModal._toggle();
        this.setState(p => ({ ...p, show: false }));
    }

    render() {
        return (
            <div id='page-delivery-payment' className="page-comp">
                <Container>
                    <Row>
                        <Col>
                            <div className='card padding w-100 mb-15'>
                                <Row>
                                    <Col xs={12} className='d-flex flex-column'>
                                        <div className='m-b'>
                                            <i className='zmdi zmdi-shopping-cart icon'></i>&nbsp;
                                             <span className='val'>{strings.orderId} : {this.state.loading ? <Skeleton className='skeleton' animation='wave' width={50} /> : <span>{this.state.uniqueId}</span>} </span>
                                        </div>
                                        <div className='m-b'>
                                            <img src={deliveryCostImage} alt='delivery' />&nbsp;
                                        <span className='val'>{strings.deliveryType} : {this.state.loading ? <Skeleton className='skeleton' animation='wave' width={50} /> : <span>{this.state.type === 0 ? strings.peyk : strings.post}</span>} </span>
                                        </div>
                                        <div className='m-b'>
                                            <i className='zmdi zmdi-money icon'></i>&nbsp;
                                            <span className='val'>{strings.deliverCost} : {this.state.loading ? <Skeleton className='skeleton' animation='wave' width={50} /> : <span>{commaThousondSeperator(this.state.price)} {strings.currency}</span>}</span>
                                        </div>
                                    </Col>

                                </Row>
                                <Row>
                                    <Col className='d-flex justify-content-end'>
                                        <Button loading={this.state.btnInProgresss} disabled={this.state.btnInProgresss} onClick={this._pay.bind(this)} className='btn-next'>
                                            {strings.payment}
                                        </Button>
                                    </Col>
                                </Row>
                            </div>

                        </Col>
                    </Row>

                </Container>
            </div>
        );
    }
}
const mapStateToProps = state => {
    return { ...state.reviewReducer, ...state.basketReducer };
}

const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
    hideInitError: () => dispatch(HideInitErrorAction())
});

export default connect(mapStateToProps, mapDispatchToProps)(DeliveryPayment);
