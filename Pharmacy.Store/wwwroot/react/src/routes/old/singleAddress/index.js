import React from 'react';
import { TextField } from '@material-ui/core';
import { Container, Row, Col, Button, Alert } from 'react-bootstrap';
import strings, { validationStrings } from '../../shared/constant';
import CustomMap from './../../shared/map';
import addressApi from './../../api/addressApi';
import Loader from './../../shared/Loader';
import Wrapper from './../../shared/purchaseDrawer';

class SingleAddress extends React.Component {
    constructor(props) {
        super(props);
        const { params } = this.props.match;
        this.state = {
            id: params.id,
            loading: params.id ? true : false,
            message: {
                variant: '',
                text: ''
            },
            address: {
                value: '',
                error: false,
                errorMessage: ''
            },
            plague: {
                value: '',
                error: false,
                errorMessage: ''
            },
            unit: {
                value: '',
                error: false,
                errorMessage: ''
            },
            mobileNumber: {
                value: '',
                error: false,
                errorMessage: ''
            },
            reciever: {
                value: '',
                error: false,
                errorMessage: ''
            },
            location: {
                lng: 51.338030,
                lat: 35.700084
            },
        }
    }
    async componentDidMount() {

        if (this.state.id) {
            let callRep = await addressApi.getSingleAddress(this.props.match.params);
            if (!callRep.success) {
                this.setState(p => ({ loading: false, message: { variant: 'danger', text: callRep.message } }));
                return;
            }
            let state = this.state;
            state.loading = false;
            state.address.value = callRep.result.address;
            state.reciever.value = callRep.result.reciever;
            state.location.lng = callRep.result.lng;
            state.location.lat = callRep.result.lat;
            this.map._onClick(state.location);
            this.setState(p => ({ ...state }));
        }
    }

    _inputChanged(e) {
        let state = this.state;
        state[e.target.id].value = e.target.value
        this.setState((p) => ({ ...state }));
    }

    _mapChangd(lng, lat) {
        this.setState((p) => ({ ...p, location: { lng: lng, lat: lat } }));
    }

    async _submit() {
        this.props.changePanel(1);
        return
        if (!this.state.address.value) {
            this.setState(p => ({ ...p, address: { ...p.address, error: true, errorMessage: validationStrings.required } }));
            return;
        }
        let callRep;
        if (this.state.id) {
            callRep = await addressApi.updateAddress({
                id: this.state.id,
                address: this.state.address,
                reciever: this.state.reciever,
                lng: this.state.lng,
                lat: this.state.lat
            });
        }
        else {
            callRep = await addressApi.addAddress({
                address: this.state.address,
                reciever: this.state.reciever,
                lng: this.state.lng,
                lat: this.state.lat
            });
        }
        if (!callRep.success) {
            this.setState(p => ({ loading: false, message: { variant: 'danger', text: callRep.message } }));
            return;
        }
        this.setState(p => ({ ...p, loading: false }));
        this.props.history.goBack();
    }
    render() {
        return (
            <div className="single-address-page">
                <Container>
                    <Row>
                        <Col>
                            <div className="form-group">
                                {
                                    this.state.message.variant !== '' ?
                                        (<Alert variant={this.state.message.variant}>
                                            <p className="text-center">{this.state.message.text}</p>
                                        </Alert>) : null
                                }
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <div className="form-group">
                                <TextField
                                    error={this.state.address.error}
                                    id="address"
                                    variant='outlined'
                                    label={strings.address}
                                    value={this.state.address.value}
                                    onChange={this._inputChanged.bind(this)}
                                    helperText={this.state.address.errorMessage}
                                    style={{ fontFamily: 'iransans' }}
                                />
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={3} sm={3}>
                            <div className="form-group">
                                <TextField
                                    error={this.state.plague.error}
                                    id="plague"
                                    type='number'
                                    variant='outlined'
                                    label={strings.plague}
                                    value={this.state.plague.value}
                                    onChange={this._inputChanged.bind(this)}
                                    helperText={this.state.plague.errorMessage}
                                    style={{ fontFamily: 'iransans' }}
                                />
                            </div>

                        </Col>
                        <Col xs={3} sm={3}>
                            <div className="form-group">
                                <TextField
                                    error={this.state.unit.error}
                                    id="unit"
                                    type='number'
                                    variant='outlined'
                                    label={strings.unit}
                                    value={this.state.unit.value}
                                    onChange={this._inputChanged.bind(this)}
                                    helperText={this.state.unit.errorMessage}
                                    style={{ fontFamily: 'iransans' }}
                                />
                            </div>

                        </Col>
                        <Col xs={6} sm={6}>
                            <div className="form-group">
                                <TextField
                                    error={this.state.mobileNumber.error}
                                    id="mobileNumber"
                                    type='number'
                                    variant='outlined'
                                    label={strings.mobileNumber}
                                    value={this.state.mobileNumber.value}
                                    onChange={this._inputChanged.bind(this)}
                                    helperText={this.state.mobileNumber.errorMessage}
                                    style={{ fontFamily: 'iransans' }}
                                />
                            </div>

                        </Col>
                        <Col>
                            <div className="form-group">
                                <TextField
                                    error={this.state.reciever.error}
                                    id="reciever"
                                    variant='outlined'
                                    label={strings.reciever}
                                    value={this.state.reciever.value}
                                    onChange={this._inputChanged.bind(this)}
                                    helperText={this.state.reciever.errorMessage}
                                    style={{ fontFamily: 'iransans' }}
                                />
                            </div>
                            <div className="map-group">
                                <CustomMap ref={(map) => this.map = map} location={this.state.location} onChanged={this._mapChangd.bind(this)} />
                            </div>
                            <div className="btn-group">
                                <Button variant="outline-info" onClick={this._submit.bind(this)}>{strings.submit}</Button>
                            </div>
                        </Col>
                    </Row>
                    <Loader show={this.state.loading} />
                </Container>
            </div>

        );
    }
}

export default SingleAddress;
