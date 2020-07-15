import React from 'react';
import { LogInAction } from '../../redux/actions/authenticationAction';
import { ShowToastAction } from '../../redux/actions/toastAction';
import { connect } from 'react-redux'
import { Link } from 'react-router-dom';
import strings from '../../shared/constant';
import { Alert, Button } from 'react-bootstrap';
import { TextField } from '@material-ui/core';

class LogIn extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            message: {
                variant: '',
                text: ''
            },
            username: {
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
    
    _inputChanged(e){
        let state =  this.state;
        state[e.target.id].value =  e.target.value
        this.setState((p) => ({ ...state }));
    }

    submit(e) {
        if (this.state.username.value !== 'admin' || this.state.password.value !== 'admin') {
            this.setState(p => ({
                ...p,
                message: {
                    variant: 'danger',
                    text: strings.wrongUsernameOrPassword
                }
            }))
            return;
        }
        this.props.logIn('xxx', 1, this.state.username.value);
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
                        error={this.state.username.error}
                        id="username"
                        label={strings.username}
                        value={this.state.username.value}
                        onChange={this._inputChanged.bind(this)}
                        helperText={this.state.username.errorMessage}
                        style={{ fontFamily: 'iransans' }}
                        variant="outlined"
                    />
                </div>
                <div className="form-group">
                    <TextField
                        error={this.state.password.error}
                        id="password"
                        type='password'
                        label={strings.password}
                        value={this.state.password.value}
                        onChange={this._inputChanged.bind(this)}
                        helperText={this.state.password.errorMessage}
                        variant="outlined"
                    />
                </div>
                <div className="btn-group">
                    <Button variant="outline-info" onClick={this.submit.bind(this)}>{strings.logIn}</Button>
                </div>
                <div className="recover-password">
                    <Link to="/recoverPassword"><small>{strings.forgotMyPassword}</small></Link>
                </div>
            </div>
        );
    }
}

const mapStateToProps = state => {
    return { ...state.headerReducer, ...state.authenticationReducer };
}

const mapDispatchToProps = dispatch => ({
    logIn: (token, userId, username) => { dispatch(LogInAction(token, userId, username)); },
    showToast: (title, body) => dispatch(ShowToastAction(title, body))
});

export default connect(mapStateToProps, mapDispatchToProps)(LogIn);
