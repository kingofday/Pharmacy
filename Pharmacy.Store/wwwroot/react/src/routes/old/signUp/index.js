import React from 'react';
import { SignUpAction } from '../../redux/actions/authenticationAction';
import { connect } from 'react-redux'

import strings from '../../shared/constant';
import { Alert, Button } from 'react-bootstrap';
import { TextField } from '@material-ui/core';

class SignUp extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            message: {
                variant: '',
                text: ''
            },
            fullname: {
                value: '',
                error: false,
                errorMessage: ''
            },
            mobile: {
                value: '',
                error: false,
                errorMessage: ''
            },
            email: {
                value: '',
                error: false,
                errorMessage: ''
            },
            password: {
                value: '',
                error: false,
                errorMessage: ''
            }
        }
    }
    fullnameChanged(e) {
        let v = e.target.value;
        this.setState((p) => ({ ...p, username: { ...p.username, value: v } }));
    }
    mobileChanged(e) {
        let v = e.target.value;
        this.setState((p) => ({ ...p, username: { ...p.username, value: v } }));
    }
    emailChanged(e) {
        let v = e.target.value;
        this.setState((p) => ({ ...p, username: { ...p.username, value: v } }));
    }
    passwordChanged(e) {
        let v = e.target.value;
        this.setState((p) => ({ ...p, password: { ...p.password, value:v  } }));
    }

    submit(e) {
        if (this.state.username.value !== 'admin' || this.state.password.value !== 'admin') {
            this.setState(p => ({
                ...p,
                message: {
                    variant:'danger',
                    text:strings.wrongUsernameOrPassword
                }
            }))
            return;
        }
        this.props.signUp('xxx', 1, this.state.username.value);
    }

    render() {
        return (
            <div className="login-wrapper">
                <div className="form-group">
                    {
                        this.state.message.variant !== '' ?
                            (<Alert variant={this.state.message.variant}>
                                <p className="text-center">{this.state.message.text}</p>
                            </Alert>) : null
                    }
                </div>
                <div className="form-group">
                    <TextField
                        error={this.state.fullname.error}
                        id="fullname"
                        label={strings.fullname}
                        value={this.state.fullname.value}
                        onChange={this.fullnameChanged.bind(this)}
                        helperText={this.state.fullname.errorMessage}
                        style={{ fontFamily: 'iransans' }}
                    />
                </div>
                <div className="form-group">
                    <TextField
                        error={this.state.email.error}
                        id="email"
                        label={strings.email}
                        value={this.state.email.value}
                        onChange={this.fullnameChanged.bind(this)}
                        helperText={this.state.email.errorMessage}
                        style={{ fontFamily: 'iransans' }}
                    />
                </div>
                <div className="form-group">
                    <TextField
                        error={this.state.mobile.error}
                        id="mobile"
                        type='number'
                        label={strings.mobile}
                        value={this.state.mobile.value}
                        onChange={this.fullnameChanged.bind(this)}
                        helperText={this.state.mobile.errorMessage}
                        style={{ fontFamily: 'iransans' }}
                    />
                </div>
                <div className="form-group">
                    <TextField
                        error={this.state.password.error}
                        id="password"
                        type='passwrod'
                        label={strings.password}
                        value={this.state.password.value}
                        onChange={this.passwordChanged.bind(this)}
                        helperText={this.state.password.errorMessage}
                    />
                </div>
                <div className="btn-group">
                    <Button variant="outline-info" onClick={this.submit.bind(this)}>{strings.signUp}</Button>
                </div>
            </div>
        );
    }
}

const mapStateToProps = state => {
    return { ...state.headerReducer, ...state.authenticationReducer };
}

const mapDispatchToProps = dispatch => ({
    signUp: (fullname, mobile,email, password) => { dispatch(SignUpAction(fullname, mobile,email, password)); }
});

export default connect(mapStateToProps, mapDispatchToProps)(SignUp);
