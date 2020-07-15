import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import strings from './../../shared/constant';
import DiscountBadg from './../../shared/discountBadg';
import Counter from './../../shared/counter';
import Header from './../../shared/header';
import { commaThousondSeperator } from './../../shared/utils';
import { UpdateBasketAction, RemoveFromBasketAction } from './../../redux/actions/basketAction';
import ConfirmModal from './../../shared/confirm';
import { HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import emptyBasketImage from './../../assets/images/empty-basket.png';

class Basket extends React.Component {

    async componentDidMount() {
        this.props.hideInitError();
        console.log(this.props.items);
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
    render() {
        if (this.props.items.length == 0)
            return (<div className='basket-page with-header'>
                <Header goBack={this.props.history.goBack} />
                <div className='empty'>
                    {/* <i className='zmdi zmdi-mood-bad'></i> */}
                    <img className='m-b' src={emptyBasketImage} alt='basket' />
                    <span>{strings.basketIsEmpty}</span>
                </div>

            </div>);
        else
            return (
                <div className='basket-page with-header'>
                    <Header goBack={this.props.history.goBack} />
                    <Container className='basket-wrapper'>
                        {this.props.items.map((x) => (
                            <Row key={x.id}>
                                <Col>
                                    <div className='item'>
                                        {x.imgUrl ?
                                            (<div className='img-wrapper'>
                                                <Link to={`product/${x.id}`}><img src={x.imgUrl} alt='img item' /></Link>
                                            </div>) : null}

                                        <div className='info'>
                                            <div className='name m-b'>
                                                <h2 className='hx'>{x.name}</h2>
                                                <DiscountBadg discount={x.discount} />
                                            </div>
                                            <Counter id={x.id} className='m-b' count={x.count} onChange={this._changeCount.bind(this)} />
                                            <div className='price-wrapper'>
                                                <span className='price'>{commaThousondSeperator((x.realPrice * x.count).toString())}<small className='currency'> {strings.currency}</small></span>
                                                <button onClick={this._delete.bind(this, x.id, x.name)} className='btn-delete'><i className='zmdi zmdi-delete'></i></button>
                                            </div>
                                        </div>
                                    </div>

                                </Col>
                            </Row>
                        ))}
                    </Container>
                    <div className='footer-row'>
                        <Container className='total-price-wrapper'>
                            <Row>
                                <Col className='d-flex align-items-center justify-content-end'>
                                    <small>{strings.totalSum}:&nbsp;</small>
                                    <span className='total-price'>
                                        {commaThousondSeperator(this.props.items.reduce((p, c) => (p + c.realPrice * c.count), 0).toString())}
                                    </span>
                                    <small>&nbsp;{strings.currency}</small>

                                </Col>
                            </Row>
                        </Container>
                        <Link className='btn-next d-block' to='/completeInformation'>
                            <span>{strings.continuePurchase}</span>
                        </Link>
                    </div>
                    <ConfirmModal ref={(i) => this.modal = i} onDelete={this._confirmDelete.bind(this)} />
                </div>
            );
    }
}
const mapStateToProps = state => {
    return { ...state.basketReducer };
}

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction()),
    updateBasket: (id, count) => dispatch(UpdateBasketAction(id, count)),
    removeFromBasket: (id) => dispatch(RemoveFromBasketAction(id))
});

export default connect(mapStateToProps, mapDispatchToProps)(Basket);
