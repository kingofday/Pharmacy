import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Container, Row, Col, DropdownButton, Dropdown } from 'react-bootstrap';
import { LogOutAction, SetNexPage } from './../../../redux/actions/authAction';
import logoImage from './../../../assets/images/layout/logo.png';
import strings from './../../constant';

class TopHeader extends React.Component {
    constructor(props) {
        super(props);
        this.state = { animate: false };
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
    _handleLogOut() {
        this.props.logOut();
        this.props.setAuthNexPage("/");
    }
    render() {
        return (
            <section id='comp-top-header'>
                <Container>
                    <Row>
                        <Col xs={6} sm={6} className='logo-wrapper'>
                            <img src={logoImage} alt='pharmacy logo' />
                        </Col>
                        <Col xs={6} sm={6} className='auth-wrapper'>
                            {this.props.authenticated ? null : <Link to='/auth'>
                                <i className='auth-icon icon zmdi zmdi-account'></i>
                            </Link>}

                            <Link to='/basket' className={this.state.animate ? 'ripple-loader' : ''}>
                                <i className='icon zmdi zmdi-shopping-cart'></i>
                            </Link>
                            {this.props.authenticated ? <DropdownButton id="ddl-options" title={this.props.fullname}>
                                <Dropdown.Item href='/profile'>
                                    <i className='zmdi zmdi-account'></i>&nbsp;{strings.profile}
                                </Dropdown.Item>
                                <Dropdown.Item href='/orderHistory'>
                                    <i className=' zmdi zmdi-format-list-bulleted'></i>&nbsp;{strings.orders}
                                </Dropdown.Item>
                                <Dropdown.Item>
                                    <button className='log-out' onClick={this._handleLogOut.bind(this)}>
                                        <i className=' zmdi zmdi-power'></i>&nbsp;{strings.logOut}
                                    </button>
                                </Dropdown.Item>
                            </DropdownButton>

                                : null}
                        </Col>
                    </Row>
                </Container>
            </section>

        );
    }
}

const mapStateToProps = (state, ownProps) => {
    return { ...ownProps, ...state.basketReducer, ...state.authReducer };
}

const mapDispatchToProps = dispatch => ({
    logOut: () => dispatch(LogOutAction()),
    setAuthNexPage: (nextPage) => dispatch(SetNexPage(nextPage))
});

export default connect(mapStateToProps, mapDispatchToProps)(TopHeader);