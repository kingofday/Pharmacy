import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import { Link, Redirect } from 'react-router-dom';
import strings from './../../shared/constant';
import DiscountBadg from './../../shared/discountBadg';
import Counter from './../../shared/counter';
import { commaThousondSeperator } from './../../shared/utils';
import { UpdateBasketAction, RemoveFromBasketAction } from './../../redux/actions/basketAction';
import ConfirmModal from './../../shared/confirm';
import { HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import { SetNexPage } from "../../redux/actions/authAction";
import emptyBasketImage from './../../assets/images/empty-basket.png';

class Basket extends React.Component {
    state = {
        redirect: null
    }
    async componentDidMount() {
        this.props.hideInitError();
    }

    _changeCount(id, count) {
        this.props.updateBasket(id, count);
    }

    _delete(id, name) {
        this.modal._toggle(id, strings.areYouSureForDeleteingProduct.replace('##name##', name));

    }
    _confirmDelete(id) {
        this.props.removeFromBasket(id);
    }
    _goToNext() {
        console.log(this.props.authenticated);
        if (this.props.authenticated)
            this.setState(p => ({ ...p, redirect: '/selectAddress' }));
        else {
            this.props.setAuthNextPage('/selectAddress');
            this.setState(p => ({ ...p, redirect: '/auth' }));
        }
    }
    render() {
        if (this.state.redirect)
            return <Redirect to={this.state.redirect} />
        else if (this.props.items.length == 0)
            return (<div className='basket-page with-header'>
                <div className='empty'>
                    {/* <i className='zmdi zmdi-mood-bad'></i> */}
                    <img className='m-b' src={emptyBasketImage} alt='basket' />
                    <span>{strings.basketIsEmpty}</span>
                </div>

            </div>);
        else
            return (
                <div className='basket-page page-comp'>
                    <Container className='basket-wrapper'>
                        {this.props.items.map((x, idx) => (
                            <Row key={idx}>
                                <Col>
                                    <div className='item'>
                                        <Row className='w-100'>
                                            <Col xs={9} sm={9} lg={6}>
                                                <div className='main-info'>
                                                    {x.thumbnailImageUrl ?
                                                        (<div className='img-wrapper'>
                                                            <Link to={`product/${x.id}`}><img src={x.thumbnailImageUrl} alt='img item' /></Link>
                                                        </div>) : null}

                                                    <div className='info'>
                                                        <h2 className='hx'>{x.nameFa}</h2>
                                                        <Counter id={x.id} className='m-b' count={x.count} onChange={this._changeCount.bind(this)} />
                                                        <span className='price'>{commaThousondSeperator((x.realPrice * x.count).toString())}<small className='currency'> {strings.currency}</small></span>
                                                    </div>
                                                </div>

                                            </Col>
                                            <Col className='d-none d-lg-flex' lg={3}>
                                                <div className='extra-info'>
                                                    <label className='mb-15'>{strings.identifier}: {x.uniqueId}</label>
                                                    <label className='mb-15'>{strings.unit}: {x.unitName}</label>
                                                    <label>{x.nameEn}</label>
                                                </div>

                                            </Col>
                                            <Col xs={3} className='d-flex end-col' lg={3}>
                                                <div><DiscountBadg discount={x.discount} /></div>
                                                <div><button onClick={this._delete.bind(this, x.id, x.nameFa)} className='btn-delete'><i className='zmdi zmdi-delete'></i></button></div>
                                            </Col>
                                        </Row>
                                    </div>

                                </Col>
                            </Row>
                        ))}
                        <Row>
                            <Col>
                                <div className='card padding total-row'>
                                    <div className='total-wrapper'>
                                        <span>{strings.totalSum}:&nbsp;</span>
                                        <span className='total-price'>
                                            {commaThousondSeperator(this.props.items.reduce((p, c) => (p + c.realPrice * c.count), 0).toString())}
                                        </span>
                                        <small>&nbsp;{strings.currency}</small>
                                    </div>
                                    <button className='btn-next d-block' onClick={this._goToNext.bind(this)}>
                                        <span>{strings.continuePurchase}</span>
                                    </button>
                                </div>
                            </Col>
                        </Row>
                    </Container>

                    <ConfirmModal ref={(i) => this.modal = i} onDelete={this._confirmDelete.bind(this)} />
                </div>
            );
    }
}
const mapStateToProps = state => {
    return { ...state.basketReducer, authenticated: state.authReducer.authenticated };
}

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction()),
    updateBasket: (id, count) => dispatch(UpdateBasketAction(id, count)),
    removeFromBasket: (id) => dispatch(RemoveFromBasketAction(id)),
    setAuthNextPage: () => dispatch(SetNexPage())
});

export default connect(mapStateToProps, mapDispatchToProps)(Basket);
