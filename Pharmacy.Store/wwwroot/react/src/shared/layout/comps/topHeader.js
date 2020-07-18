import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import { LogOutAction } from './../../../redux/actions/authenticationAction';
import logoImage from './../../../assets/images/logo.png';
import strings from './../../constant';

export default class TopHeader extends React.Component {
    constructor(props) {
        super(props);
        this.state = { animate: false }
    }
    componentDidUpdate(prevProps) {
        if (this.props.items.length !== prevProps.items.length) {
            this.setState(p => ({ ...p, animate: true }));
            let context = this;
            setTimeout(() => {
                context.setState(p => ({ ...p, animate: false }));
            }, 1000);
        }
    }
    _handleLogOut(){
        this.props.logOut();
        this.props.history.push("/");
    }
    render() {
        return (
            <section id='comp-top-header'>
            <Container>
                <Row>
                    <Col xs={6} sm={6}>
                        <image src={logoImage} alt='pharmacy logo' />
                    </Col>
                    <Col xs={6} sm={6}>
                        <Link to={this.props.route}>
                            <i className='default-i zmdi zmdi-shopping-cart'></i>
                        </Link>
                        <Link to={this.props.token ? '/profile' : '/auth'}>
                            <i className='default-i zmdi zmdi-account'></i>
                        </Link>
                        {this.props.token ? <button className='log-out' onClick={this._handleLogOut.bind(this)}>
                            <i className='default-i zmdi zmdi-power'></i>
                        </button> : null}
                    </Col>
                </Row>
            </Container>
            </section>

        );
    }
}

const mapStateToProps = state => {
    return { ...state.basketReducer, ...state.authenticationReducer };
}

const mapDispatchToProps = dispatch => ({
    logOut: () => dispatch(LogOutAction())
});

export default connect(mapStateToProps, mapDispatchToProps)(BasketIcon);