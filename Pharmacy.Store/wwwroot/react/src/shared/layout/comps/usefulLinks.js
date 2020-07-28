import React from 'react';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import strings from './../../constant';

export default class UsefulLinks extends React.Component {

    render() {
        const info = {
            tel: '02155750099',
            email: 'info@hillavas.com',
            address: 'زعفرانیه- بن بست بهار- پلا 3'
        }
        return (
            <section id='comp-useful-links' className='mb-15'>
                <Container>
                    <Row>
                        <Col col={12}>
                            <div className='card padding md-15'>
                                <Row>
                                    <Col xs={12} sm={6} lg={3} className='mb-15 direction-column text-center'>
                                        <h5 className='hx'>شغل و مراقبت</h5>
                                        <p>داروخانه آنلاین بانک محصولات دارویی است. در داروخانه انواع داروها، وسایل آرایشی و بهداشتی عرضه می شود</p>
                                        <Link className='more' to='#'>
                                            {strings.more}...
                                        </Link>
                                    </Col>
                                    <Col xs={12} sm={6} lg={3} className='mb-15 direction-column text-center'>
                                        <h5 className='hx'>ارتباط با ما</h5>
                                        <p>
                                            <a className='info' href={`tel:${info.tel}`}><i className='icon zmdi zmdi-phone'></i><span> {info.tel}</span></a>
                                            <a className='info' href={`mailto:${info.email}`}><i className='icon zmdi zmdi-email'></i><span> {info.email}</span></a>
                                            <label className='info'><i className='icon zmdi zmdi-pin'></i><span> {info.address}</span></label>
                                        </p>
                                    </Col>
                                    <Col xs={12} sm={6} lg={3} className='mb-15 direction-column text-center'>
                                        <h5 className='hx'>پیگیری آخرین اخبار</h5>
                                        <p>لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. </p>
                                        <Link className='more' to='#'>
                                            {strings.more}...
                                        </Link>
                                    </Col>
                                    <Col xs={12} sm={6} lg={3} className='mb-15 direction-column text-center'>
                                        <h5 className='hx'>سوالات متداول</h5>
                                        <p>لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. </p>
                                        <Link className='more' to='#'>
                                            {strings.more}...
                                        </Link>
                                    </Col>
                                </Row>
                            </div>
                        </Col>
                    </Row>
                </Container>
            </section>


        );
    }
}