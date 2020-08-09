import React from 'react';
import { connect } from 'react-redux'
import { Link } from 'react-router-dom';
import { Row, Col, Alert } from 'react-bootstrap';
import { LogInAction } from './../../../redux/actions/authAction';
import { ShowToastAction } from './../../../redux/actions/toastAction';
import strings, { validationStrings } from './../../../shared/constant';
import { validate } from './../../../shared/utils';
import { TextField } from '@material-ui/core';
import srvAuth from './../../../service/srvAuth';

const inputs = ['username', 'password'];

class LogIn extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isValid: false,
            disableBtn: false,
            showModal: false,
            message: {
                variant: '',
                text: ''
            }
        };
        for (let i = 0; i < inputs.length; i++)
            this.state[inputs[i]] = {
                value: '',
                error: false,
                errorMessage: ''
            };
    }

    _inputChanged(e) {
        let state = this.state;
        state[e.target.id].value = e.target.value;
        state[e.target.id].error = false;
        state[e.target.id].errorMessage = '';
        this.setState((p) => ({ ...state }));
    }

    _validate() {
        let isValid = true;
        let state = this.state;
        for (let i = 0; i < inputs.length; i++) {
            let k = inputs[i];
            if (!state[k].value) {
                state[k].error = true;
                state[k].errorMessage = validationStrings.required;
                this.setState(p => ({ ...state }));
                isValid = false;
                continue;
            }
            let msg = null;
            switch (k) {
                case 'username':
                    if (validate.mobileNumber(state[k])) msg = validationStrings.invalidMobileNumber;
                    break;
                case 'password':
                    if (state[k].length < 5 || state[k].length > 50) msg = validationStrings.passwordInvalidLength;
                    break;
            }
            if (msg) {
                state[k].error = true;
                state[k].errorMessage = msg;
                this.setState(p => ({ ...state }));
                isValid = false;
            }
        }
        if (!isValid) return isValid;
        return isValid

    }

    _submit(e) {
        if (!this._validate()) return;
        this.setState(p => ({ ...p, disableBtn: true }));
        let model = {};
        for (let i = 0; i < inputs.length; i++)
            model[inputs[i]] = this.state[inputs[i]].value;
        srvAuth.signIn({})
        this.props.logIn('xxx', 1, this.state.username.value);
    }

    render() {
        return (
            <div id="comp-login" className='card padding'>
                <Row className='ltr-elm'>
                    <Col xs={12} sm={12} md={{ span: 4, offset: 4 }} className='flex-column'>
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
                            <button className='text-center w-100' onClick={this._submit.bind(this)}>{strings.logIn}</button>
                        </div>
                        <div className="recover-password text-center">
                            <Link to="/recoverPassword"><small>{strings.forgotMyPassword}</small></Link>
                        </div>
                    </Col>
                </Row>

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
