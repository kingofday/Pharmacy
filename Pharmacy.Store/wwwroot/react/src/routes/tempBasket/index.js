import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';
import { connect } from 'react-redux';
import { Link, Redirect } from 'react-router-dom';
import strings from './../../shared/constant';
import DiscountBadg from './../../shared/discountBadg';
import Counter from './../../shared/counter';
import ConfirmModal from './../../shared/confirm';
import { commaThousondSeperator } from './../../shared/utils';
import { SetWholeBasketAction, SetPrescriptiontIdAction } from './../../redux/actions/basketAction';
import { HideInitErrorAction, ShowInitErrorAction } from "../../redux/actions/InitErrorAction";
import emptyBasketImage from './../../assets/images/empty-basket.png';
import srvPrescription from './../../service/srvPrescription';

class TempBasket extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            items: []
        };
        this._isMounted = true;
        this.prescriptionId = this.props.match.params.id;
    }

    async _fetchData() {
        let getItems = await srvPrescription.getItems(this.prescriptionId);
        if (!this._isMounted) return;
        if (getItems.success) {
            this.props.setWholeBasket(getItems.result);
            this.setState(p => ({ ...p, items: getItems.result, loading: false }));
        }
        else this.props.showInitError(this._fetchData.bind(this));
    }

    async componentDidMount() {
        this.props.hideInitError();
        this.props.setPrescriptiontId(this.prescriptionId);
        await this._fetchData();
    }
    componentWillUnmount() {
        this._isMounted = false;
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
            return (<div className='page-comp' id='page-temp-basket'>
                <div className='empty'>
                    {/* <i className='zmdi zmdi-mood-bad'></i> */}
                    <img className='m-b' src={emptyBasketImage} alt='basket' />
                    <span>{strings.basketIsEmpty}</span>
                </div>

            </div>);
        else
            return (
                <div className='page-comp' id='page-temp-basket'>
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
                                                            <Link to={`product/${x.drugId}`}><img src={x.thumbnailImageUrl} alt='img item' /></Link>
                                                        </div>) : null}

                                                    <div className='info'>
                                                        <h2 className='hx'>{x.nameFa}</h2>
                                                        <div className='mb-15'>{strings.count}: {x.count}</div>
                                                        {/* <Counter id={x.drugId} className='m-b' count={x.count} onChange={this._changeCount.bind(this)} /> */}
                                                        <span className='price'>{commaThousondSeperator((x.realPrice * x.count).toString())}<small className='currency'> {strings.currency}</small></span>
                                                    </div>
                                                </div>

                                            </Col>
                                            <Col className='d-none d-lg-flex' lg={4}>
                                                <div className='extra-info'>
                                                    <label className='mb-15'>{strings.identifier}: {x.uniqueId}</label>
                                                    <label className='mb-15'>{strings.unit}: {x.unitName}</label>
                                                    <label>{x.nameEn}</label>
                                                </div>

                                            </Col>
                                            <Col xs={3} className='d-flex end-col' lg={2}>
                                                <div><DiscountBadg discount={x.discount} /></div>
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
                </div>
            );
    }
}
const mapStateToProps = state => {
    return { ...state.basketReducer, authenticated: state.authReducer.authenticated };
}

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction()),
    showInitError: (fetchData) => dispatch(ShowInitErrorAction(fetchData)),
    setWholeBasket: (items) => dispatch(SetWholeBasketAction(items)),
    setPrescriptiontId: (id) => dispatch(SetPrescriptiontIdAction(id))
});

export default connect(mapStateToProps, mapDispatchToProps)(TempBasket);
