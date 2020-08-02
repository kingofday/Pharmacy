import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col } from 'react-bootstrap';
import { Paper, Tabs, Tab, Box } from '@material-ui/core';
import strings from '../../shared/constant';
import Steps from './../../shared/steps';
import Login from './comps/logIn';

class Auth extends React.Component {
    state = {
        panel: 'signup'
    }
    _handleChange = (event, panel) => {
        this.setState(p => ({ ...p, panel: panel }));
    };
    render() {
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
                                <Tab value='signup' label={strings.logIn} />
                                <Tab value='login' label={strings.signUp} />
                            </Tabs>
                        </Paper>
                        <div
                            role="tabpanel"
                            hidden={this.state.panel !== 'signup'}
                            id='wrapped-tabpanel-signup'
                            aria-labelledby='wrapped-tab-signup'>
                            <Login />
                        </div>
                        <div
                            role="tabpanel"
                            hidden={this.state.panel !== 'login'}
                            id='wrapped-tabpanel-login'
                            aria-labelledby='wrapped-tab-login'>
                            <Box p={3}>
                                2
                                </Box>
                        </div>
                    </Col>
                </Row>
            </Container>
        );
    }
}

const mapStateToProps = state => {
    return { ...state.basketReducer };
}

// const mapDispatchToProps = dispatch => ({
//     logIn: (token, userId, username) => { dispatch(LogInAction(token, userId, username)); },
//     showToast: (title, body) => dispatch(ShowToastAction(title, body))
// });

export default connect(mapStateToProps, null)(Auth);
