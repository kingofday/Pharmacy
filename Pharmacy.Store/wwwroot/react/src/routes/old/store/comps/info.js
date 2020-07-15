import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import strings from './../../../shared/constant';
import Skeleton from '@material-ui/lab/Skeleton';
import storeApi from '../../../api/storeApi';
import { ShowInitErrorAction} from '../../../redux/actions/InitErrorAction';
import { connect } from 'react-redux';
import BasketIcon from './../../../shared/basketIcon';
import { Link } from 'react-router-dom';

class Info extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: '',
            logoUrl: '',
            loading: true
        };
        this._isMounted = true;
    }
    async _fetchData() {
        let storeRep = await storeApi.getSingleStore(this.props.id);
        console.log('after call');
        console.log(storeRep);
        if(!this._isMounted) return;
        if (storeRep.success) this.setState(p => ({ ...p, ...storeRep.result, loading: false }));
        else this.props.showInitError(this._fetchData.bind(this));
    }

    async componentDidMount() {
        await this._fetchData();
    }
    componentWillUnmount(){
        this._isMounted = false;
    }
    render() {
        return (
            <div className="info">
                <Container>
                    <Row className="m-b">
                        <Col className='wrapper'>
                            <div className="logo">
                                {!this.state.loading ? <img src={this.state.logoUrl} alt='logo' /> : <Skeleton variant='circle' width={80} height={80} />}
                            </div>
                            <div className='name-wrapper'>
                                {!this.state.loading ? <h2 className='name m-b'>{this.state.name}</h2> : <Skeleton className='m-b' variant='text' width={100} height={30} />}
                                <div className='basket-wrapper'>
                                    <BasketIcon />
                                    <Link className='contactus' to='/contactus'>{strings.contactus}</Link>
                                </div>
                            </div>
                        </Col>
                    </Row>

                </Container>
            </div >
        );
    }
}

const mapStateToProps = (state, ownProps) => {
    return { ...ownProps };
}

const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData) => dispatch(ShowInitErrorAction(fetchData))
});

export default connect(mapStateToProps, mapDispatchToProps)(Info);