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

class DeliveryPayment extends React.Component {
    state = {
        loading: true,
        uniqueId: 0,
        type: '',
        price: 0,
        btnInProgresss: false,
        gatewayUrl: '',
    };
    _isMounted = true;

    async componentDidMount() {
        this.props.hideInitError();
        await this._fetchData();
    }

    async _fetchData() {
        this.setState(p => ({ ...p, loading: true }));
        const { params } = this.props.match;
        let apiRep = await srvDelivery.getPrice(params.id);
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
        // this.setState(p => ({ ...p, btnInProgresss: true }));
        // let submit = await srvOrder.add({
        //     items: this.props.items,
        //     address: this.props.address,
        //     deliveryType: this.props.delivery.id,
        //     comment: this.props.comment
        // });
        // this.setState(p => ({ ...p, btnInProgresss: false }));
        // if (submit.success)
        //     window.open(submit.result.url, '_self');

        // else {
        //     if (submit.result && submit.result.basketChanged) {
        //         this.setState(p => ({ ...p, gatewayUrl: submit.result.url }));
        //         this.changedProductModal._toggle();
        //         this.props.changeBasket(submit.result.drugs);
        //     }
        //     else toast(submit.message, { type: toast.TYPE.ERROR });
        // }

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
                        <Col xs={12}>
                            <div className='card padding w-100 mb-15'>
                                <div className='m-b'>
                                    <i className='zmdi zmdi-shopping-cart icon'></i>&nbsp;
                                    <span className='val'>{strings.orderId} : {this.state.loading ? <Skeleton animation='wave' width={50} /> : <span>{this.state.uniqueId}</span>} </span>
                                </div>
                                <div className='m-b'>
                                    <img src={deliveryCostImage} alt='delivery' />&nbsp;
                                    <span className='val'>{strings.deliveryType} : {this.state.loading ? <Skeleton animation='wave' width={50} /> : <span>{this.state.type === 0 ? strings.peyk : strings.post}</span>} </span>
                                </div>
                                <div className='m-b'>
                                    <i className='zmdi zmdi-money icon'></i>&nbsp;
                                    <span className='val'>{strings.deliverCost} : {this.state.loading ? <Skeleton animation='wave' width={50} /> : <span>{commaThousondSeperator(this.state.price)} {strings.currency}</span>}</span>
                                </div>
                            </div>
                        </Col>

                    </Row>
                    <Row>
                        <Col>
                            <button className='btn-next' onClick={this._pay.bind(this)}>
                                {strings.payment}
                                {this.state.btnInProgresss ? <Spinner animation="border" size="sm" /> : null}
                            </button>
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
