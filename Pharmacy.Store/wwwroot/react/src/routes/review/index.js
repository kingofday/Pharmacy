import React from 'react';
import { Spinner, Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import { Link, Redirect } from 'react-router-dom';

import strings from './../../shared/constant';
import DiscountBadg from './../../shared/discountBadg';
import srvOrder from './../../service/srvOrder';
import { commaThousondSeperator } from './../../shared/utils';
import { ShowInitErrorAction, HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import { ChangedBasketItemsAction } from './../../redux/actions/basketAction';
import deliveryCostImage from './../../assets/images/delivery-cost.svg';
import discountImage from './../../assets/images/discount.svg';
import { toast } from 'react-toastify';
import ProductsChangedModal from './comps/ProductsChangedModal';

class Review extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: null,
            totalPrice: 0,
            discount: 0,
            currency: '',
            btnInProgresss: false,
            gatewayUrl: ''
        };
        console.log(this.props.totalPrice);
    }

    async componentDidMount() {

        this.props.hideInitError();
    }

    async _pay() {
        this.setState(p => ({ ...p, btnInProgresss: true }));
        if (this.props.prescriptionId) {
            // let submit = await srvOrder.submitTempBasket(this.props.basketId, this.props.address, this.props.reciever, this.props.recieverMobileNumber, this.props.deliveryId);
            // if (submit.success) {
            //     this.props.setBasketId(null);
            //     window.open(submit.result.url, '_self');
            // }
            // else toast(submit.message, { type: toast.TYPE.ERROR });
        }
        else {
            let submit = await srvOrder.add({
                items: this.props.items,
                address: this.props.address,
                deliveryType: this.props.delivery.id,
                comment: this.props.comment
            });
            this.setState(p => ({ ...p, btnInProgresss: false }));
            if (submit.success)
                window.open(submit.result.url, '_self');

            else {
                if (submit.result && submit.result.basketChanged) {
                    this.setState(p => ({ ...p, gatewayUrl: submit.result.url }));
                    this.changedProductModal._toggle();
                    this.props.changeBasket(submit.result.drugs);
                }
                else toast(submit.message, { type: toast.TYPE.ERROR });
            }
        }

    }

    _goToBasket() {
        this.setState(p => ({ ...p, redirect: '/basket' }));
    }

    _continue() {
        this.changedProductModal._toggle();
        this.setState(p => ({ ...p, show: false }));
    }

    render() {
        if (this.state.redirect) return <Redirect to={this.state.redirect} />
        return (
            <div id='page-review'>
                <Container className='basket-wrapper'>
                    {this.props.items.map((x, idx) => (
                        <div className='item w-100' key={idx}>
                            <Row className='w-100'>
                                <Col xs={12} sm={12} md={6} className='mb-15 d-flex'>
                                    {x.thumbnailImageUrl ?
                                        (<div className='img-wrapper'>
                                            <Link to={`product/${x.drugId}`}><img src={x.thumbnailImageUrl} alt='img item' /></Link>
                                        </div>) : null}

                                    <div className='info'>
                                        <h4 className='hx-fa mb-15'>{x.nameFa}</h4>
                                        <h5 className='hx-en'>{x.nameEn}</h5>
                                        {x.discount > 0 ? <div><DiscountBadg discount={x.discount} /></div> : null}

                                    </div>

                                </Col>
                                <Col xs={12} sm={12} md={6} className='info'>
                                    <h4 className='count m-b'>{strings.count}: {x.count}</h4>
                                    <h4 className='price'><strong className='val'>{commaThousondSeperator((x.count * x.realPrice).toString())}</strong>{strings.currency}</h4>
                                </Col>
                            </Row>
                        </div>

                    ))}
                    <Row>
                        <Col className='total-wrapper'>
                            <div className='cost m-b'>
                                <img src={deliveryCostImage} alt='delivery' />&nbsp;
                                    <span className='val'>{strings.deliveryType} : {this.props.delivery.name}</span>
                            </div>

                            <div className='discount m-b'>
                                <img src={discountImage} alt='discount' />&nbsp;
                                    <span className='val'>{strings.discount} : {this.props.totalDiscount} {strings.currency}</span>
                            </div>

                            <div className='price m-b'>
                                <span>{strings.priceToPay} : </span>
                                <span className='val'>{commaThousondSeperator(this.props.totalPrice)}</span>
                                <span>{strings.currency}</span>
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
                <ProductsChangedModal ref={modal => this.changedProductModal = modal} show={this.state.show} continue={this._continue.bind(this)} goToBasket={this._goToBasket.bind(this)} />
            </div>
        );
    }
}
const mapStateToProps = state => {
    return { ...state.reviewReducer, ...state.basketReducer };
}

const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
    hideInitError: () => dispatch(HideInitErrorAction()),
    changeBasket: (products) => dispatch(ChangedBasketItemsAction(products))
});

export default connect(mapStateToProps, mapDispatchToProps)(Review);
