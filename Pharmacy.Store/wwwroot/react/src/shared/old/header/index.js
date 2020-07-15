import React from 'react';
import BasketIcon from './../../shared/basketIcon';
import { Container, Row, Col } from 'react-bootstrap';

export default class Header extends React.Component {
    _goBack() {
        this.props.goBack();
    }
    render() {
        return (
            <div className='header-comp'>
                <Container>
                    <Row>
                        <Col className='wrapper'>
                            {this.props.hasTitle ? <h2 className='heading'>{this.props.title}</h2> : <BasketIcon />}
                            <button className='btn-back' onClick={this._goBack.bind(this)}>
                                <i className='zmdi zmdi-chevron-left'></i>
                            </button></Col>
                    </Row>
                </Container>

            </div>
        );
    }
}
