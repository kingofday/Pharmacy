import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col } from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';
import Heading from './../../shared/heading/heading';
import strings from './../../shared/constant';
import { Redirect, Link } from 'react-router-dom';
import { ShowInitErrorAction, HideInitErrorAction } from './../../redux/actions/InitErrorAction';
import srvOrder from './../../service/srvOrder';
import { commaThousondSeperator } from './../../shared/utils';

class OrderHistory extends React.Component {

    state = {
        loading: true,
        pageNumber: 1,
        items: [],
        collapse: {}
    };

    async _fetchData() {
        this.setState(p => ({ ...p, loading: true }));
        let getOrders = await srvOrder.getHistory(this.state.pageNumber);
        this.setState(p => ({ ...p, loading: false }));
        if (!getOrders.success) {
            this.props.showInitError(this._fetchData.bind(this), getOrders.message);
            return;
        }
        this.setState(p => ({ ...p, items: getOrders.result, pageNumber: (getOrders.result.length > 0 ? p.pageNumber + 1 : p.pageNumber) }));
    }

    _toggle(id) {
        let collapse = this.state.collapse;
        collapse[id] = !collapse[id];
        this.setState(p => ({ ...p, collapse: { ...collapse } }));
    }

    async componentDidMount() {
        this.props.hideInitError();
        await this._fetchData();
    }

    render() {
        if (this.state.redirect) return <Redirect to={this.state.redirect} />;
        return (
            <div id='page-order-history' className="page-comp">
                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className='card padding w-100'>
                                <Heading title={strings.orders} />
                                <div id='#items'>
                                    {this.state.loading ? null : <div id='th'> <Row className='w-100'>
                                        <Col xs={12} sm={1}>#</Col>
                                        <Col xs={12} sm={3}>{strings.orderId}</Col>
                                        <Col xs={12} sm={3}>{strings.status}</Col>
                                        <Col xs={12} sm={2}>{strings.insertDate}</Col>
                                        <Col xs={12} sm={3}>{strings.price}({strings.currency})</Col>
                                    </Row></div>}
                                    {this.state.loading ? [0, 1, 2, 3].map((x) => <div key={x} className='item mb-15'><Skeleton className='w-100' variant="rect" height={30} /></div>) :
                                        this.state.items.map((x, idx) => <div key={idx} onClick={() => this._toggle(idx)} className={'item mb-15 ' + (this.state.collapse[idx] ? 'collapsed' : '')}>
                                            <Row className='w-100 mb-15'>
                                                <Col xs={12} sm={1}>{idx + 1}</Col>
                                                <Col xs={12} sm={3}>{x.uniqueId}</Col>
                                                <Col xs={12} sm={3}>{x.status}{x.needDeliveryPayment ? <Link to={`/deliveryPayment/${x.orderId}`}><small>&nbsp;({strings.payDeliveryPrice})</small></Link> : null}</Col>
                                                <Col xs={12} sm={2}>{x.insertDate}</Col>
                                                <Col xs={12} sm={3} className='last-col'>
                                                    <span>{commaThousondSeperator(x.totalPrice)}</span>
                                                    <i className={"icon zmdi zmdi-" + (this.state.collapse[idx] ? 'chevron-up' : 'chevron-down')}></i>
                                                </Col>
                                            </Row>
                                            <div className={'details ' + (this.state.collapse[idx] ? '' : 'd-none')}>
                                                <Row >
                                                    {x.items.map((oi, oiIdx) =>
                                                        <Col key={oiIdx} xs={12} sm={4} llg={3} xl={2}>
                                                            {oi.thumbnailImageUrl ? <img className='img-item' src={oi.thumbnailImageUrl} /> : null}
                                                            <span>{oi.nameFa}({oi.uniqueId})</span>
                                                        </Col>
                                                    )}
                                                </Row>


                                            </div>
                                        </div>)}
                                </div>
                            </div>
                        </Col>
                    </Row>

                </Container>
            </div >
        );
    }

}
// const mapStateToProps = state => {
//     return { ...state.productsReducer };
// }

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction()),
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message))
});

export default connect(null, mapDispatchToProps)(OrderHistory);