import React from 'react';
import { connect } from 'react-redux';
import strings from '../../shared/constant';
import { Container, Row, Col } from 'react-bootstrap';
import greenBasketImage from './../../assets/images/green-basket.svg';
import redBasketImage from './../../assets/images/red-basket.svg';
import Skeleton from '@material-ui/lab/Skeleton';
import { ShowInitErrorAction, HideInitErrorAction } from "../../redux/actions/InitErrorAction";
import { ClearBasketAction } from "../../redux/actions/basketAction";
import Steps from './../../shared/steps';

class AfterGateway extends React.Component {
    constructor(props) {
        super(props);
        const { params } = this.props.match;
        this.state = {
            loading: false,
            success: params.status === '1',
            transId: params.transId
        }
    }
    componentDidMount() {
        if (this.state.success) {
            this.props.clearBasket();
        }
    }

    render() {
        return (
            <div id="page-after-gateway" className="page-comp">
                <Container className='basket-wrapper'>
                    <Row>
                        <Col xs={12}>
                            <div className='content card padding'>
                                {this.state.loading ? <Skeleton variant='circle' height={107} width={107} /> :
                                    <img className='img-basket' src={this.state.success ? greenBasketImage : redBasketImage} alt='basket' />}

                                {this.state.loading ? <Skeleton className='main-message' width={120} height={25} variant='text' /> :
                                    <span className={'main-message ' + (this.state.success ? 'success' : 'error')}>
                                        {this.state.success ? strings.thankYouForPurchase : strings.purchaseFailed}
                                    </span>}
                                {this.state.loading ? <Skeleton className='hint' width={120} variant='text' /> :
                                    <span className='hint'>
                                        {this.state.success ? strings.successfulOrder : strings.retryPlease}
                                    </span>}

                                {this.state.loading ? <Skeleton className='trace-id-text' width={120} variant='text' /> :
                                    <span className='trace-id-text'>
                                        {strings.orderTraceIdIs}
                                    </span>}

                                {this.state.loading ? <Skeleton className='m-b trace-id' width={50} variant='text' /> :
                                    <span className='m-b trace-id'>
                                        {this.state.transId}
                                    </span>}
                                {(!this.state.loading && this.state.success) ?
                                    <span>
                                        {strings.callYouLater}
                                    </span> : null}
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