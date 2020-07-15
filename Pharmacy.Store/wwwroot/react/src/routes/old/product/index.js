import React from 'react';
import { Button, Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import { ShowInitErrorAction, HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import productApi from './../../api/productApi';
import strings from './../../shared/constant';
import Skeleton from '@material-ui/lab/Skeleton';
import Slider from './comps/slider';
import Header from './../../shared/header';
import DiscountBadg from './../../shared/discountBadg';
import { AddToBasketAction } from './../../redux/actions/basketAction';
import { commaThousondSeperator, checkLocalStorage } from './../../shared/utils';
import Counter from './../../shared/counter';
import addToBasketImage from './../../assets/images/add-to-basket.svg';

class Product extends React.Component {
    constructor(props) {
        super(props);
        let count = 1;
        const item = this.props.items.find(x => x.id == this.props.match.params.id);
        if (item) count = item.count;
        this.state = {
            loading: true,
            count: count,
            product: {
                name: '',
                price: 0,
                realPrice: 0,
                discount: 0,
                likeCount: 0,
                slides: [],
                desc: ''
            }
        };
        this._isMounted = true;
    }

    async _fetchData() {
        const { params } = this.props.match;
        let apiRep = await productApi.getSingleProduct(params.id);
        if (!this._isMounted) return;
        if (!apiRep.success) {
            this.props.showInitError(this._fetchData.bind(this), apiRep.message);
            return;
        }
        this.setState(p => ({ ...p, product: { ...apiRep.result }, loading: false }));
    }

    async componentDidMount() {
        checkLocalStorage();
        this.props.hideInitError();
        await this._fetchData();
    }

    componentWillUnmount() {
        this._isMounted = false;
    }

    _changeCount(id, count) {
        this.setState(p => ({ ...p, count: count }));
    }

    _addToBasket() {
        this.props.addToBasket(this.state.product, this.state.count);
    }

    render() {
        const p = this.state.product;
        return (
            <div className='product-page with-header'>
                <Header goBack={this.props.history.goBack} />
                <Slider slides={this.state.product.slides} />
                <Container className='info'>
                    <Row className='name-row'>
                        <Col>
                            {this.state.loading ? <div className='name'><Skeleton variant='rect' height={20} width='60%' /></div> :
                                <h1 className='name'>{p.name}</h1>}
                        </Col>
                    </Row>
                    <Row className="details-row m-b">
                        <Col>
                            {this.state.loading ? [1, 2, 3, 4].map(x => <Skeleton key={x} variant='text' />) :
                                <p className='desc'>{p.description}</p>}
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={6} sm={6} className='d-flex align-items-center'>
                            <Counter id={this.state.product.id} count={this.state.count} id={this.state.product.id} onChange={this._changeCount.bind(this)} />
                        </Col>
                        <Col xs={6} sm={6}>

                            {this.state.loading ? [1, 2].map(x => <Skeleton key={x} variant='text' height={20} />) :
                                (<div className='price-wrapper'>
                                    {
                                        p.discount ? (
                                            <div>
                                                <span className='price'>{commaThousondSeperator(p.price.toString())} {strings.currency}</span>
                                                <DiscountBadg discount={p.discount} />
                                            </div>) : null
                                    }
                                    <div className='real-price-wrapper'>
                                        <span className='real-price'>{commaThousondSeperator(p.realPrice.toString())}</span>
                                        <span className='currency'>{strings.currency}</span>
                                    </div>
                                </div>)}
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Button disabled={this.state.loading} className={"btn-purchase btn-next " + (window.innerWidth > 576 ? "fab" : "")} onClick={this._addToBasket.bind(this)}>
                                {`${strings.add} ${strings.to} ${strings.basket}`}
                                &nbsp;
                                <img src={addToBasketImage} alt='add to basket' />
                            </Button>
                        </Col>
                    </Row>
                </Container>


            </div>
        );
    }
}
const mapStateToProps = state => {
    return { ...state.basketReducer };
}

const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
    hideInitError: () => dispatch(HideInitErrorAction()),
    addToBasket: (product, count) => dispatch(AddToBasketAction(product, count))
    // sendProductIno: (payload) => dispatch(SendProductInoAction(payload))
});

export default connect(mapStateToProps, mapDispatchToProps)(Product);
