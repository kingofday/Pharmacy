import React from 'react';
import { connect } from 'react-redux';
import { TextField } from '@material-ui/core';
import { Container, Row, Col, Alert } from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';
import { Radio, FormControlLabel, RadioGroup } from '@material-ui/core';

import Heading from './../../shared/heading/heading';
import Steps from './../../shared/steps';
import Button from './../../shared/Button';
import strings from './../../shared/constant';
import { Redirect } from 'react-router-dom';
import { ShowInitErrorAction, HideInitErrorAction } from './../../redux/actions/InitErrorAction';
import srvDelivery from './../../service/srvDelivery';

export default class orderHistory extends React.Component {

    state = {
        loading: true,
        items: []
    };

    async _fetchData() {
        this.setState(p => ({ ...p, loading: true }));
        let delivery = await srvDelivery.get();
        console.log(delivery);
        this.setState(p => ({ ...p, loading: false }));
        if (!delivery.success) {
            this.props.showInitError(this._fetchData.bind(this), delivery.message);
            return;
        }
        this.setState(p => ({ ...p, deliveryId: delivery.result[0].id.toString(), deliveryTypes: delivery.result }));
    }


    async componentDidMount() {
        this.props.hideInitError();
        await this._fetchData();
    }

    render() {
        if (this.state.redirect) return <Redirect to={this.state.redirect} />;
        return (
            <div id='page-order-history' className="page-comp">
                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className='card padding w-100'>
                                <Heading title={strings.deliveryType} />
                                <div id='#items'>
                                    {this.state.loading ? <Row key={x}>
                                        <Col xs={12} sm={1}>#</Col>
                                        <Col xs={12} sm={3}>{strings.orderId}</Col>
                                        <Col xs={12} sm={3}>{strings.status}</Col>
                                        <Col xs={12} sm={3}>{strings.insertDate}</Col>
                                        <Col xs={12} sm={3}>{strings.price}</Col>
                                    </Row> : null}
                                    {this.state.loading ? [0, 1, 2].map((x) => <Row key={x}><Col><Skeleton variant="rect" height={100} /></Col></Row>) :
                                        this.state.items.map((x, idx) => <Row key={x} >
                                            <Col xs={12}>
                                                <Col xs={12} sm={1}>{idx + 1}</Col>
                                                <Col xs={12} sm={3}>{x.uniqueId}</Col>
                                                <Col xs={12} sm={3}>{x.status}</Col>
                                                <Col xs={12} sm={3}>{x.insertDate}</Col>
                                                <Col xs={12} sm={3}>{x.totalPrice}</Col>
                                            </Col>
                                        </Row>)}
                                </div>
                                <Row>
                                    <Col>
                                        <Steps activeStep={1} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <Alert className='w-100 text-center' variant='warning'>
                                            {strings.deliveryPriceGuid}
                                        </Alert>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col xs={12} sm={12}>
                                        <Heading title={strings.deliveryType} />
                                    </Col>
                                    <Col xs={12} sm={12} className='d-flex flex-column'>
                                        {this.state.loading ? [0, 1].map((x) => <Skeleton className='mb-15' width={150} key={x} variant='rect' height={25} />) :
                                            <RadioGroup aria-label="address" name="old-address" value={this.state.deliveryId} onChange={this._selectDeliveryType.bind(this)}>
                                                {this.state.deliveryTypes.map((d) => <FormControlLabel key={d.id} value={d.id.toString()} control={<Radio color="primary" />} label={d.name} />)}
                                            </RadioGroup>}
                                    </Col>
                                    <Col xs={12}>
                                        <div className="form-group mb-0">
                                            <TextField
                                                id="comment"
                                                error={this.state.comment.error}
                                                label={strings.comment}
                                                multiline
                                                rows={3}
                                                value={this.state.comment.value}
                                                onChange={this._inputChanged.bind(this)}
                                                helperText={this.state.comment.message}
                                                variant="outlined" />
                                        </div>
                                    </Col>
                                </Row>

                                <Row>
                                    <Col xs={12} sm={12} className='d-flex justify-content-end'>
                                        <Button onClick={this._submit.bind(this)} disabled={this.state.loading} loading={this.state.btnDisabled}>
                                            {strings.continuePurchase}
                                        </Button>
                                    </Col>
                                </Row>
                            </div>
                        </Col>
                    </Row>

                </Container>
            </div >
        );
    }

}
