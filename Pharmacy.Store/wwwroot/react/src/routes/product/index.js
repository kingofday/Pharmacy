import React from 'react';
import { Container, Row, Col, Tabs, Tab } from 'react-bootstrap';
import { connect } from 'react-redux';

import Button from './../../shared/Button';
import { ShowInitErrorAction, HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import strings from './../../shared/constant';
import Skeleton from '@material-ui/lab/Skeleton';
import Slider from './comps/slider';
import DiscountBadg from './../../shared/discountBadg';
import { AddToBasketAction } from './../../redux/actions/basketAction';
import { commaThousondSeperator, checkLocalStorage } from './../../shared/utils';
import Counter from './../../shared/counter';
import addToBasketImage from './../../assets/images/add-to-basket.svg';
import srvDrug from './../../service/srvDrug';

class Product extends React.Component {
    constructor(props) {
        super(props);
        let count = 1;
        const item = this.props.items.find(x => x.id == this.props.match.params.id);
        if (item) count = item.count;
        this.state = {
            loading: true,
            count: count,
            activePanelKey: 'desc',
            product: {
                name: '',
                price: 0,
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
        let apiRep = await srvDrug.getSingle(params.id);
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
            <div id='page-drug'>
                <Container>
                    <Row>
                        <Col col={12} sm={12} md={6}>
                            <div className='card padding w-100 mb-15'>
                                <Slider slides={p.slides} />
                            </div>
                        </Col>
                        <Col col={12} sm={12} md={6}>
                            <div className='card padding w-100 mb-15'>
                                <h1 id='name' className='mb-15'>{p.nameFa}</h1>
                                <Row>
                                    <Col xs={6} className='flex-column d-flex justify-content-center'>
                                        {this.state.loading ? [1, 2].map(x => <Skeleton key={x} className='w-100 mb-15' variant='rect' height={20} />) :
                                            (<div className='price-wrapper'>
                                                {
                                                    p.discount ? (
                                                        <div className='mb-15'>
                                                            <span className='price'>{commaThousondSeperator(p.price.toString())} {strings.currency}</span>
                                                            <DiscountBadg discount={p.discount} />
                                                        </div>) : null
                                                }
                                                <div className='real-price-wrapper mb-15'>
                                                    <span className='real-price'>{commaThousondSeperator(p.realPrice.toString())}</span>
                                                    <span className='currency'>{strings.currency}</span>
                                                </div>
                                            </div>)}
                                    </Col>
                                    <Col xs={6} className='d-flex justify-content-center flex-column'>
                                        <label className='mb-15'>{strings.identifier}: {this.state.loading ? <Skeleton className='d-inline-block va-middle' height={25} width={70} /> : p.uniqueId}</label>
                                        <label className='mb-15'>{strings.category}: {this.state.loading ? <Skeleton className='d-inline-block va-middle' height={25} width={70} /> : p.categoryName}</label>
                                    </Col>
                                </Row>
                                <Row className='mb-15'>
                                    <Col xs={12} sm={12} >
                                        <label>{strings.unit}: {p.unitName}</label>
                                    </Col>
                                    <Col col={12} sm={12} className='direction-column' id='btn-add-wrapper'>
                                        <Counter id={p.id} count={this.state.count} onChange={this._changeCount.bind(this)} />
                                        <Button disabled={this.state.loading} className="btn-purchase" onClick={this._addToBasket.bind(this)}>
                                            {`${strings.add} ${strings.to} ${strings.basket}`}
                                            &nbsp;
                                            <img src={addToBasketImage} alt='add to basket' />
                                        </Button>
                                    </Col>
                                </Row>
                                <div id='tags-wrapper'></div>
                            </div>
                        </Col>
                        <Col xs={12} sm={12}>
                            <div className='card padding w-100 mb-15'>
                                <Tabs
                                    id="controlled-tab-example"
                                    activeKey={this.state.activePanelKey}
                                    onSelect={(k) => this.setState(prev => ({ ...prev, activePanelKey: k }))}>
                                    <Tab eventKey="desc" title={strings.description}>
                                        <p className='padding'>
                                            {this.state.loading ? [0, 1, 2, 3, 4].map(x => <Skeleton key={x} height={20} className='w-100' />) : p.description}
                                        </p>
                                    </Tab>
                                    <Tab eventKey="properties" title={strings.properties}>
                                        <Row>
                                            <Col xs={12} sm={6}>
                                                <div className='padding w-100'>
                                                    {this.state.loading ? null : <table className='table table-striped table-bordered'>
                                                        <tbody>
                                                            {p.properties.map(prop => <tr>
                                                                <td>{prop.name}</td>
                                                                <td>{prop.value}</td>
                                                            </tr>)}
                                                        </tbody>
                                                    </table>}
                                                </div>
                                            </Col>
                                        </Row>



                                    </Tab>
                                    <Tab eventKey="comments" title={strings.comments}>
                                        <div className='comments padding'>
                                            {this.state.loading ? null : <ul>
                                                {p.comments.map((c, idx) => <li key={idx}>
                                                    <strong>{c.fullname}</strong>
                                                    <p>{c.comment}</p>
                                                </li>)}
                                            </ul>}
                                        </div>
                                    </Tab>
                                </Tabs>

                            </div>
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
