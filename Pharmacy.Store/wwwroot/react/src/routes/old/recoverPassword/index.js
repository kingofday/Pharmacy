import React from 'react';
import { Button, Alert } from 'react-bootstrap';
import { connect } from 'react-redux';
import strings from './../../shared/constant';
import accountImage from './../../assets/images/account.png';
import { Link } from 'react-router-dom';
import { TextField } from '@material-ui/core';

class RecoverPassword extends React.Component {
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
            <div className="change-password-page">
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

export default connect(null, null)(RecoverPassword);
