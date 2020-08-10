import React from 'react';
import { connect } from 'react-redux';
import { Row, Col } from 'react-bootstrap';
import { toast } from 'react-toastify';
import { TextField } from '@material-ui/core';
import { ShowModalAction } from './../../../redux/actions/modalAction';
import strings, { validationStrings } from '../../../shared/constant';
import { validate } from './../../../shared/utils';
import Button from './../../../shared/Button';
import Confirm from './confirm';
import srvAuth from './../../../service/srvAuth';
import Modal from './../../../shared/modal';

const inputs = ['fullname', 'mobileNumber', 'email', 'newPassword', 'repeatPassword'];

class SignUp extends React.Component {
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
    _handleChange(e) {
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
            if (k !== 'email' && !state[k].value) {
                state[k].error = true;
                state[k].errorMessage = validationStrings.required;
                this.setState(p => ({ ...state }));
                isValid = false;
                continue;
            }
            let msg = null;
            switch (k) {
                case 'mobileNumber':
                    if (validate.mobileNumber(state[k])) msg = validationStrings.invalidMobileNumber;
                    break;
                case 'email':
                    if (validate.email(state[k])) msg = validationStrings.invalidEmail;
                    break;
                case 'newPassword':
                    if (state[k].length < 5 || state[k].length > 50) msg = validationStrings.passwordInvalidLength;
                    break;
                case 'repeatPassword':
                    if (state['newPassword'].value !== state[k].value) msg = validationStrings.incorrectRepeatPassword;
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
    async _submit(e) {
        if (!this._validate()) return;
        this.setState(p => ({ ...p, disableBtn: true }));
        let model = {};
        for (let i = 0; i < inputs.length; i++)
            model[inputs[i]] = this.state[inputs[i]].value;
        let signup = await srvAuth.signUp(model);
        this.setState(p => ({ ...p, disableBtn: false }));
        if (!signup.success) {
            toast(signup.message, { type: toast.TYPE.ERROR });
            return;
        }
        this.modal.toggleModal(true);
    }

    render() {
        return (
            <div id="comp-signup" className='card padding'>
                <Row className='ltr-elm'>
                    <Col xs={12} sm={12} md={{ span: 8, offset: 2 }} className='flex-column'>
                        <div className="form-group">
                            <TextField
                                error={this.state.fullname.error}
                                id="fullname"
                                name="fullname"
                                label={strings.fullname}
                                value={this.state.fullname.value}
                                onChange={this._handleChange.bind(this)}
                                helperText={this.state.fullname.errorMessage}
                                style={{ fontFamily: 'iransans' }}
                                variant="outlined"
                            />
                        </div>
                    </Col>
                    <Col xs={12} sm={12} md={{ span: 4, offset: 2 }} className='flex-column'>
                        <div className="form-group">
                            <TextField
                                error={this.state.email.error}
                                id="email"
                                name="email"
                                label={strings.email}
                                value={this.state.email.value}
                                onChange={this._handleChange.bind(this)}
                                helperText={this.state.email.errorMessage}
                                style={{ fontFamily: 'iransans' }}
                                variant="outlined"
                            />
                        </div>
                    </Col>
                    <Col xs={12} sm={12} md={{ span: 4 }} className='flex-column'>
                        <div className="form-group">
                            <TextField
                                error={this.state.mobileNumber.error}
                                id="mobileNumber"
                                name="mobileNumber"
                                placeholder="9xxxxxxxxx"
                                label={strings.mobileNumber}
                                value={this.state.mobileNumber.value}
                                onChange={this._handleChange.bind(this)}
                                helperText={this.state.mobileNumber.errorMessage}
                                style={{ fontFamily: 'iransans' }}
                                variant="outlined"
                            />
                        </div>
                    </Col>
                    <Col xs={12} sm={12} md={{ span: 4, offset: 2 }} className='flex-column'>
                        <div className="form-group">
                            <TextField
                                error={this.state.repeatPassword.error}
                                id="repeatPassword"
                                name="repeatPassword"
                                type='password'
                                label={strings.repeatPassword}
                                value={this.state.repeatPassword.value}
                                onChange={this._handleChange.bind(this)}
                                helperText={this.state.repeatPassword.errorMessage}
                                variant="outlined"
                            />
                        </div>
                    </Col>
                    <Col xs={12} sm={12} md={{ span: 4 }} className='flex-column'>
                        <div className="form-group">
                            <TextField
                                error={this.state.newPassword.error}
                                id="newPassword"
                                name="newPassword"
                                type="password"
                                label={strings.password}
                                value={this.state.newPassword.value}
                                onChange={this._handleChange.bind(this)}
                                helperText={this.state.newPassword.errorMessage}
                                variant="outlined"
                            />
                        </div>
                    </Col>
                    <Col xs={12} sm={12} md={{ span: 4, offset: 4 }} className='flex-column'>
                        <div className="btn-group">
                            <Button disabled={this.state.disableBtn} className='text-center w-100' onClick={this._submit.bind(this)}>{strings.signUp}</Button>
                        </div>
                    </Col>


                </Row>
                <Modal ref={c => this.modal = c} title={strings.confirmCode}>
                    <Confirm mobileNumber={this.state.mobileNumber.value} />
                </Modal>
            </div>
        );
    }
}

const mapStateToProps = state => {
    return { ...state.headerReducer, ...state.authenticationReducer };
}

const mapDispatchToProps = dispatch => ({
    showModal: (modal) => { dispatch(ShowModalAction(modal)); }
});

export default connect(mapStateToProps, mapDispatchToProps)(SignUp);
