import React from 'react';
import { Redirect } from "react-router-dom";
import { Button, Alert } from 'react-bootstrap';
import { connect } from 'react-redux';
import strings from './../../shared/constant';
import { TextField } from '@material-ui/core';

class ChangePassword extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            message: {
                variant: '',
                text: ''
            },
            password: {
                value: '',
                error: false,
                errorMessage: ''
            },
            confirmPassword: {
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

    _submit(e) {
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
    }


    render() {
        return (
            <div className="recover-password-page">
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
                        error={this.state.password.error}
                        id="password"
                        type="password"
                        label={strings.password}
                        value={this.state.password.value}
                        onChange={this._inputChanged.bind(this)}
                        helperText={this.state.password.errorMessage}
                        style={{ fontFamily: 'iransans' }}
                    />
                </div>
                <div className="form-group">
                    <TextField
                        error={this.state.password.error}
                        id="confirmPassword"
                        type="password"
                        label={strings.confirmPassword}
                        value={this.state.confirmPassword.value}
                        onChange={this._inputChanged.bind(this)}
                        helperText={this.state.confirmPassword.errorMessage}
                        style={{ fontFamily: 'iransans' }}
                    />
                </div>
                <div className="btn-group">
                    <Button variant="outline-info" onClick={this._submit.bind(this)}>{strings.recoverPassword}</Button>
                </div>
            </div>
        );
    }
}
// const mapStateToProps = state => {
//     return { ...state.homeReducer };
// }

// const mapDispatchToProps = dispatch => ({
// });

export default connect(null, null)(ChangePassword);
