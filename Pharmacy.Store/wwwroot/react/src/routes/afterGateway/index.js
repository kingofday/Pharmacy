import React from 'react';
import { connect } from 'react-redux';
import strings from '../../shared/constant';
import { Container, Row, Col } from 'react-bootstrap';
import greenBasketImage from './../../assets/images/green-basket.svg';
import redBasketImage from './../../assets/images/red-basket.svg';
import Skeleton from '@material-ui/lab/Skeleton';
import { ShowInitErrorAction, HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import { ClearBasketAction } from "../../redux/actions/basketAction";

class AfterGateway extends React.Component {
    constructor(props) {
        super(props);
        const { params } = this.props.match;
        this.state = {
            success: params.status === '1',
            type: params.type,
            orderId: params.orderId,
            transId: params.transId
        }
    }
    componentDidMount() {
        console.log('!!!!');
        if (this.state.success) {
            this.props.clearBasket();
        }
    }

    render() {
        console.log(this.state);
        if (this.state.type === "0") {
            return (<div id="page-after-gateway" className="page-comp">
                <Container className='basket-wrapper'>
                    <Row>
                        <Col xs={12}>
                            <div className='content card padding'>
                                <img className='img-basket' src={this.state.success ? greenBasketImage : redBasketImage} alt='basket' />

                                <span className={'main-message ' + (this.state.success ? 'success' : 'error')}>
                                    {this.state.success ? strings.thankYouForPurchase : strings.purchaseFailed.replace('{0}', this.state.orderId)}
                                </span>
                                {this.state.orderId !== "0" ? <span className='hint'>
                                    {this.state.success ? strings.successfulOrder.replace('{0}', this.state.orderId) : strings.retryPlease}
                                </span> : null}


                                <span className='trace-id-text'>
                                    {strings.orderTraceIdIs}
                                </span>
                                <span className='m-b trace-id'>
                                    {this.state.transId}
                                </span>
                                {this.state.success ?
                                    <span>
                                        {strings.callYouLater}
                                    </span> : null}
                            </div>

                        </Col>
                    </Row>
                </Container>

            </div>);
        }
        return (
            <div id="page-after-gateway" className="page-comp">
                <Container className='basket-wrapper'>
                    <Row>
                        <Col xs={12}>
                            <div className='content card padding'>
                                <img className='img-basket' src={this.state.success ? greenBasketImage : redBasketImage} alt='basket' />

                                <span className={'main-message ' + (this.state.success ? 'success' : 'error')}>
                                    {this.state.success ? strings.successPayment : strings.failedPayment}
                                </span>
                                {this.state.orderId !== "0" ? <span className='hint'>
                                    {this.state.success ? strings.successPaymentHint : strings.failedPaymentHint.replace('{0}', this.state.orderId)}
                                </span> : null}


                                <span className='trace-id-text'>
                                    {strings.orderTraceIdIs}
                                </span>
                                <span className='m-b trace-id'>
                                    {this.state.transId}
                                </span>
                            </div>

                        </Col>
                    </Row>
                </Container>

            </div>
        );
    }
}

const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
    hideInitError: () => dispatch(HideInitErrorAction()),
    clearBasket: () => dispatch(ClearBasketAction())
});
export default connect(null, mapDispatchToProps)(AfterGateway);