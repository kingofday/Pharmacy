import React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import { Paper, Tabs, Tab, Box } from '@material-ui/core';
import strings from '../../shared/constant';
import Login from './comps/SignIn';
import SignUp from './comps/signUp';

class Auth extends React.Component {
    state = {
        panel: 'signup'
    }
    _handleChange = (event, panel) => {
        this.setState(p => ({ ...p, panel: panel }));
    };
    render() {
        if (this.props.goToNextPage)
            return <Redirect to={this.props.nextPage} />
        return (
            <Container id="page-auth" className=' page-comp'>
                <Row>
                    <Col>
                        <Paper className='mb-15'>
                            <Tabs
                                value={this.state.panel}
                                onChange={this._handleChange.bind(this)}
                                indicatorColor="primary"
                                textColor="primary"
                                centered
                            >
                                <Tab value='signup' label={strings.signUp} icon={<i className='zmdi zmdi-account-add icon'></i>} />
                                <Tab value='login' label={strings.logIn} icon={<i className='zmdi zmdi-sign-in icon'></i>} />
                            </Tabs>
                        </Paper>
                        <div
                            role="tabpanel"
                            hidden={this.state.panel !== 'signup'}
                            id='wrapped-tabpanel-signup'
                            aria-labelledby='wrapped-tab-signup'>
                            <SignUp />

                        </div>
                        <div
                            role="tabpanel"
                            hidden={this.state.panel !== 'login'}
                            id='wrapped-tabpanel-login'
                            aria-labelledby='wrapped-tab-login'>
                            <Login />
                        </div>
                    </Col>
                </Row>
            </Container>
        );
    }
}

const mapStateToProps = (state, ownProps) => {
    return { ...ownProps,...state.authReducer };
}

// const mapDispatchToProps = dispatch => ({
//     logIn: (token, userId, username) => { dispatch(LogInAction(token, userId, username)); },
//     showToast: (title, body) => dispatch(ShowToastAction(title, body))
// });

export default connect(mapStateToProps, null)(Auth);
