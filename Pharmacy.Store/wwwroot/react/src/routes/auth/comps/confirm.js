import React from 'react';
import { connect } from 'react-redux'
import { TextField } from '@material-ui/core';
import strings, { validationStrings } from './../../../shared/constant';
import { Row, Col, Alert } from 'react-bootstrap';
import { LogInAction, GoToNextPage } from './../../../redux/actions/authAction';
import Button from './../../../shared/Button';
import srvAuth from './../../../service/srvAuth';

class Confirm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            message: {
                variant: '',
                text: ''
            },
            code: {
                value: '',
                error: false,
                errorMessage: ''
            }
        }
    }

    _inputChanged(e) {
        let state = this.state;
        state[e.target.id].value = e.target.value;
        state[e.target.id].error = false;
        state[e.target.id].errorMessage = '';
        this.setState((p) => ({ ...state, message: { variant: '', text: '' } }));
    }

    async _submit(e) {
        if (!this.state.code.value) {
            this.setState(p => ({ ...p, code: { ...p.code, error: true, errorMessage: validationStrings.required } }));
            return;
        }
        let confirm = await srvAuth.confirm(this.props.mobileNumber, this.state.code.value);
        if (!confirm.success) {
            this.setState(p => ({ ...p, message: { variant: 'danger', text: confirm.message } }));
            return;
        }
        this.props.logIn(confirm.result);
        this.props.goToNextPage();
    }
    async _resendSMS() {
        this.setState(p => ({ ...p, resending: true }));
        let resend = await srvAuth.resendSMS(this.props.mobileNumber);
        this.setState(p => ({ ...p, resending: false }));
        if (!resend.success) {
            this.setState(p => ({ ...p, message: { variant: 'danger', text: resend.message } }));
            return;
        }
    }

    render() {
        return (
            <div id="comp-confirm" className='card padding'>
                <Row className='ltr-elm'>
                    <Col xs={12} sm={12} className='text-center m-b'>
                        <p className='text-center'>{strings.mobileNumberConfirmDescription}</p>
                    </Col>
                    <Col xs={12} sm={12} md={{ span: 6, offset: 3 }} className='flex-column'>
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
                                error={this.state.code.error}
                                id="code"
                                name="code"
                                label={strings.confirmCode}
                                value={this.state.code.value}
                                onChange={this._inputChanged.bind(this)}
                                helperText={this.state.code.errorMessage}
                                style={{ fontFamily: 'iransans' }}
                                variant="outlined"
                            />
                        </div>
                        <div className="btn-group mb-15">
                            <Button disabled={this.state.disableBtn} loading={this.state.disableBtn} className='text-center w-100' onClick={this._submit.bind(this)}>{strings.confirm}</Button>
                        </div>
                        <div className="btn-group justify-content-center">
                            <Button disabled={this.state.resending} loading={this.state.resending} className='btn-confirm-retry' onClick={this._resendSMS.bind(this)}>{strings.sendAgain}</Button>
                        </div>
                    </Col>
                </Row>

            </div>
        );
    }
}

// const mapStateToProps = state => {
//     return { ...state.headerReducer, ...state.authenticationReducer };
// }

const mapDispatchToProps = dispatch => ({
    logIn: (model) => dispatch(LogInAction(model)),
    goToNextPage: () => dispatch(GoToNextPage())
});

export default connect(null, mapDispatchToProps)(Confirm);
