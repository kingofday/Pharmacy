import React from 'react';
//import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
//import strings from './../../constant';

export default class Footer extends React.Component {

    render() {

        return (
            <footer id='comp-footer' className='mb-15'>
                <div id='copyright'>
                    <Container>
                        <Row>
                            <Col col={12}>
                                <p className='text-center'>
                                    @ComprRight 2020
                                </p>
                            </Col>
                        </Row>
                    </Container>
                </div>
            </footer>


        );
    }
}