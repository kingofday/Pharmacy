import React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import { Container, Row, Col, Spinner } from 'react-bootstrap';
import { TextField } from '@material-ui/core';
import { validate } from './../../shared/utils';
import strings, { validationStrings } from '../../shared/constant';
import Header from './../../shared/header';
import Steps from './../../shared/steps';
import orderSrv from './../../service/orderSrv';

class Completeinformation extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: null,
            loading: true,
            btnInProgresss: false,
            fullname: {
                value: '',
                error: false,
                message: ''
            },
            mobileNumber: {
                value: '',
                error: false,
                message: ''
            },
            description: {
                value: '',
                error: false,
                message: ''
            }
        }
    }

    _inputChanged(e) {
        let state = this.state;
        if (e.target.name) state[e.target.name].value = e.target.value;
        else state[e.target.id].value = e.target.value;
        this.setState((p) => ({ ...p, ...state }));
    }

    componentDidMount() {
        if (this.props.items.length === 0)
            toast(strings.doPurchaseProcessAgain, {
                type: toast.TYPE.INFO,
                onClose: function () {
                    this.setState(p => ({ ...p, redirect: '/basket' }));
                }.bind(this)
            });

        let info = orderSrv.getInfo();
        if (info != null) {
            this.setState(p => ({
                ...p,
                fullname: { ...p.fullname, value: info.fullname },
                mobileNumber: { ...p.mobileNumber, value: info.mobileNumber },
                description: { ...p.description, value: info.description }
            }));
        }
    }

    async _submit() {
        if (!this.state.fullname.value) {
            this.setState(p => ({ ...p, fullname: { ...p.fullname, error: true, message: validationStrings.required } }));
            return;
        }
        if (!this.state.mobileNumber.value) {
            this.setState(p => ({ ...p, mobileNumber: { ...p.mobileNumber, error: true, message: validationStrings.required } }));
            return;
        }
        if (!validate.mobileNumber(this.state.mobileNumber.value)) {
            this.setState(p => ({ ...p, mobileNumber: { ...p.mobileNumber, error: true, message: validationStrings.invalidMobileNumber } }));
            return;
        }
        // if (!this.state.description.value) {
        //     this.setState(p => ({ ...p, description: { ...p.description, error: true, message: validationStrings.required } }));
        //     return;
        // }
        this.setState(p => ({ ...p, btnInProgresss: true }));
        let rep = await orderSrv.addInfo({
            fullname: this.state.fullname.value,
            mobileNumber: parseInt(this.state.mobileNumber.value),
            description: this.state.description.value
        });
        this.setState(p => ({ ...p, btnInProgresss: false }));
        if (!rep.success)
            toast(rep.message, { type: toast.TYPE.ERROR });
        else
            this.setState(p => ({ redirect: '/selectAddress' }));

    }

    render() {
        if (this.state.redirect)
            return (<Redirect to={this.state.redirect} />);
        return (
            <div className="complete-information-page with-header">
                <Header goBack={this.props.history.goBack} />
                <Steps />
                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className="form-group">
                                <TextField
                                    error={this.state.fullname.error}
                                    id="fullname"
                                    label={strings.fullname}
                                    value={this.state.fullname.value}
                                    onChange={this._inputChanged.bind(this)}
                                    helperText={this.state.fullname.message}
                                    style={{ fontFamily: 'iransans' }}
                                    variant="outlined" />
                            </div>

                        </Col>
                        <Col xs={12}>
                            <div className="form-group">
                                <TextField
                                    error={this.state.mobileNumber.error}
                                    id="mobileNumber"
                                    type='number'
                                    className='ltr-input'
                                    label={strings.mobileNumber}
                                    value={this.state.mobileNumber.value}
                                    onChange={this._inputChanged.bind(this)}
                                    helperText={this.state.mobileNumber.message}
                                    style={{ fontFamily: 'iransans' }}
                                    variant="outlined" />
                            </div>
                        </Col>

                        <Col xs={12}>
                            <div className="form-group">
                                <TextField
                                    id="description"
                                    label={strings.description}
                                    multiline
                                    rows={4}
                                    value={this.state.description.value}
                                    onChange={this._inputChanged.bind(this)}
                                    variant="outlined" />
                            </div>
                        </Col>

                    </Row>

                </Container>

                <button className='btn-next' onClick={this._submit.bind(this)}>
                    {strings.continuePurchase}
                    {this.state.btnInProgresss ? <Spinner animation="border" size="sm" /> : null}
                </button>
            </div>
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

export default connect(mapStateToProps, null)(Completeinformation);
