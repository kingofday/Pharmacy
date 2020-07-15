import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import queryString from 'query-string'

import strings from './../../shared/constant';
import DiscountBadg from './../../shared/discountBadg';
import Header from './../../shared/header';
import ConfirmModal from './../../shared/confirm';
import { commaThousondSeperator } from './../../shared/utils';
import { UpdateBasketAction, SetBasketRouteAction, SetWholeBasketAction } from './../../redux/actions/basketAction';
import { HideInitErrorAction, ShowInitErrorAction } from "../../redux/actions/InitErrorAction";
import { SetTempBasketIdAction } from "../../redux/actions/tempBasketAction";
import emptyBasketImage from './../../assets/images/empty-basket.png';
import orderApi from './../../api/orderApi';

class TempBasket extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            items: []
        };
        this._isMounted = true;
        this.basketId = this.props.match.params.basketId;
    }

    async _fetchData() {
        let getItems = await orderApi.getBasketItems(this.basketId);
        if (!this._isMounted) return;
        if (getItems.success) {
            this.props.setWholeBasket(getItems.result);
            this.setState(p => ({ ...p, items: getItems.result, loading: false }));
        }
        else this.props.showInitError(this._fetchData.bind(this));
    }

    async componentDidMount() {
        this.props.hideInitError();
        this.props.setBasketRoute(`/tempBasket/${this.basketId}`)
        this.props.setBasketId(this.basketId);
        await this._fetchData();
    }
    componentWillUnmount() {
        this._isMounted = false;
    }
    _changeCount(id, count) {
        this.props.updateBasket(id, count);
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
                        {this.state.loading ? [0, 1, 2].map(i => (<Skeleton key={i} style={{ marginTop: 15 }} variant='rect' height={80} />)) :
                            this.state.items.map((x) => (
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
                                                <div className='price-wrapper'>
                                                    <span className='price'>{commaThousondSeperator((x.price * x.count).toString())}<small className='currency'> {strings.currency}</small></span>
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
                                        {commaThousondSeperator(this.props.items.reduce((p, c) => (p + c.price * c.count), 0).toString())}
                                    </span>
                                    <small>&nbsp;{strings.currency}</small>

                                </Col>
                            </Row>
                        </Container>
                        <Link className='btn-next d-block' to='../completeInformation'>
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
    showInitError: (fetchData) => dispatch(ShowInitErrorAction(fetchData)),
    updateBasket: (id, count) => dispatch(UpdateBasketAction(id, count)),
    setBasketRoute: (route) => dispatch(SetBasketRouteAction(route)),
    setWholeBasket: (items) => dispatch(SetWholeBasketAction(items)),
    setBasketId: (id) => dispatch(SetTempBasketIdAction(id))
});

export default connect(mapStateToProps, mapDispatchToProps)(TempBasket);
