import React from 'react';
import { Row, Col } from 'react-bootstrap';
import { UpdateBasketAction, RemoveFromBasketAction} from './../../redux/actions/basketAction';

class BasketItem extends React.Component {
    render() {
        const item = this.props.item;
        return (
            <div className='comp-basket-item card w-100'>
                <Row className=''>
                    <Col xs={9} sm={9} lg={6}>
                        <div className='main-info'>
                            {item.thumbnailImageUrl ?
                                (<div className='img-wrapper'>
                                    <Link to={`product/${item.drugId}`}><img src={item.thumbnailImageUrl} alt='img item' /></Link>
                                </div>) : null}

                            <div className='info'>
                                <h2 className='hx'>{item.nameFa}</h2>
                                {this.props.fixed ? <h6>{strings.count}: {item.count}</h6> : <Counter id={item.drugId} className='m-b' count={item.count} onChange={this._changeCount.bind(this)} />}
                                <span className='price'>{commaThousondSeperator((item.realPrice * item.count).toString())}<small className='currency'> {strings.currency}</small></span>
                            </div>
                        </div>

                    </Col>
                    <Col className='d-none d-lg-flex' lg={3}>
                        <div className='extra-info'>
                            <label className='mb-15'>{strings.identifier}: {item.uniqueId}</label>
                            <label className='mb-15'>{strings.unit}: {item.unitName}</label>
                            <label>{x.nameEn}</label>
                        </div>

                    </Col>
                    <Col xs={3} className='d-flex end-col' lg={3}>
                        <div><DiscountBadg discount={item.discount} /></div>
                        <div><button onClick={this._delete.bind(this, item.drugId, item.nameFa)} className='btn-delete'><i className='zmdi zmdi-delete'></i></button></div>
                    </Col>
                </Row>
            </div>

        );
    }
}

// const mapStateToProps = state => {
//     return { authenticated: state.authReducer.authenticated, prescriptionId: state.reviewReducer.prescriptionId };
// }

const mapDispatchToProps = dispatch => ({
    updateBasket: (id, count) => dispatch(UpdateBasketAction(id, count)),
    removeFromBasket: (id) => dispatch(RemoveFromBasketAction(id))
});

export default connect(null, mapDispatchToProps)(BasketItem);