import React, { Component } from 'react';
import { connect } from 'react-redux';

import { Container, Row, Col } from 'react-bootstrap';
import { toast } from 'react-toastify';
import { TextField, Checkbox, FormControlLabel } from '@material-ui/core';
import strings, { validationStrings } from './../../shared/constant';
import { validate } from './../../shared/utils';
import Button from './../../shared/Button';
import srvUser from './../../service/srvUser';
import Heading from './../../shared/heading/heading';
import { UpdateProfileAction } from './../../redux/actions/authAction';
const inputs = ['fullname', 'email', 'newPassword', 'repeatPassword'];

class Profile extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isValid: false,
            disableBtn: false,
            changePassword: false,
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
        this.state.user = srvUser.getUserInfo();
        this.state.fullname.value = this.state.user.result.fullname;
        this.state.email.value = this.state.user.result.email;
    }

    _handleChange(e) {
        let state = this.state;
        state[e.target.id].value = e.target.value;
        state[e.target.id].error = false;
        state[e.target.id].errorMessage = '';
        this.setState((p) => ({ ...state }));
    }

    _handlePassword() {
        let val = this.state.changePassword;
        if (val)
            this.setState(p => ({
                ...p,
                changePassword: false,
                newPassword: {
                    ...p.newPassword,
                    value: '',
                    error: false,
                    errorMessage: ''
                },
                repeatPassword: {
                    ...p.repeatPassword,
                    value: '',
                    error: false,
                    errorMessage: ''
                },
            }));

        else this.setState(p => ({ ...p, changePassword: true }));
    }
    _validate() {
        let isValid = true;
        let state = this.state;
        for (let i = 0; i < inputs.length; i++) {
            let k = inputs[i];
            let msg = null;
            switch (k) {
                case 'fullname':
                    if (!state[k].value) msg = validationStrings.required;
                    break;
                case 'email':
                    if (!validate.email(state[k].value)) msg = validationStrings.invalidEmail;
                    break;
                case 'newPassword':
                    if (this.state.changePassword && !state[k].value) msg = validationStrings.required;
                    if (this.state.changePassword && state[k].length < 5 || state[k].length > 50) msg = validationStrings.passwordInvalidLength;
                    break;
                case 'repeatPassword':
                    if (this.state.changePassword && !state[k].value) msg = validationStrings.required;
                    if (this.state.changePassword && state['newPassword'].value !== state[k].value) msg = validationStrings.incorrectRepeatPassword;
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
        delete model.repeatPassword;
        let signup = await srvUser.updateProfile(model);
        this.setState(p => ({ ...p, disableBtn: false }));
        if (!signup.success) {
            toast(signup.message, { type: toast.TYPE.ERROR });
            return;
        }
        toast(strings.successfullOperation, { type: toast.TYPE.SUCCESS });
        this.props.updateProfile(model);
    }

    render() {
        return (
            <div id='page-profile' className="page-comp">
                <Container>
                    <Row>
                        <Col sm={12}>
                            <div className='padding card'>
                                <Row className='ltr-elm'>
                                    <Col xs={12} sm={12} md={{ span: 8, offset: 2 }} className='flex-column'>
                                        <Heading title={strings.profile} className='justify-content-center' />
                                    </Col>
                                </Row>
                                <br />
                                <Row className='ltr-elm'>
                                    <Col xs={12} sm={12} md={{ span: 4, offset: 2 }} className='flex-column'>
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
                                    <Col xs={12} sm={12} md={{ span: 4 }} className='flex-column'>
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
                                    <Col xs={12} sm={12} md={{ span: 8, offset: 2 }} className='flex-column'>
                                        <div className="form-group text-right">
                                            <FormControlLabel
                                                control={
                                                    <Checkbox
                                                        checked={this.state.changePassword}
                                                        onChange={() => this._handlePassword()}
                                                        name="changePassword"
                                                        color="primary"
                                                    />
                                                }
                                                label={strings.changePassword}
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
                                                disabled={!this.state.changePassword}
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
                                                disabled={!this.state.changePassword}
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
                                            <Button disabled={this.state.disableBtn} loading={this.state.disableBtn} className='text-center w-100' onClick={this._submit.bind(this)}>{strings.submit}</Button>
                                        </div>
                                    </Col>


                                </Row>
                            </div>
                        </Col>
                    </Row>
                </Container>
            </div>

        );
    };

}

// const mapStateToProps = state => {
//     return { ...state.basketReducer };
// }

const mapDispatchToProps = dispatch => ({
    updateProfile: (user) => dispatch(UpdateProfileAction(user)),
});

export default connect(null, mapDispatchToProps)(Profile);