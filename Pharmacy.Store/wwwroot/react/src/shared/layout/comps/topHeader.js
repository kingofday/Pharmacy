import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import { LogOutAction } from './../../../redux/actions/authAction';
import logoImage from './../../../assets/images/layout/logo.png';

class TopHeader extends React.Component {
    constructor(props) {
        super(props);
        this.state = { animate: false }
    }
    componentDidUpdate(prevProps) {
        console.log('fired');
        if (this.props.items.length !== prevProps.items.length) {
            console.log('changed');
            this.setState(p => ({ ...p, animate: true }));
            let context = this;
            setTimeout(() => {
                context.setState(p => ({ ...p, animate: false }));
            }, 1000);
        }
    }
    _handleLogOut() {
        this.props.logOut();
        window.location.href='/';
    }
    render() {
        console.log('topHeader');
        console.log(this.props.authenticated);
        return (
            <section id='comp-top-header'>
                <Container>
                    <Row>
                        <Col xs={6} sm={6} className='logo-wrapper'>
                            <img src={logoImage} alt='pharmacy logo' />
                        </Col>
                        <Col xs={6} sm={6} className='auth-wrapper'>
                            <Link to={this.props.authenticated ? '/profile' : '/auth'}>
                                <i className='default-i zmdi zmdi-account'></i>
                            </Link>
                            <Link to='/basket' className={this.state.animate ? 'ripple-loader' : ''}>
                                <i className='default-i zmdi zmdi-shopping-cart'></i>
                            </Link>
                            {this.props.authenticated ? <button className='log-out' onClick={this._handleLogOut.bind(this)}>
                                <i className='default-i zmdi zmdi-power'></i>
                            </button> : null}
                        </Col>
                    </Row>
                </Container>
            </section>

        );
    }
}

const mapStateToProps = (state,ownProps) => {
    return { ...ownProps,...state.basketReducer, ...state.authReducer };
}

const mapDispatchToProps = dispatch => ({
    logOut: () => dispatch(LogOutAction())
});

export default connect(mapStateToProps, mapDispatchToProps)(TopHeader);