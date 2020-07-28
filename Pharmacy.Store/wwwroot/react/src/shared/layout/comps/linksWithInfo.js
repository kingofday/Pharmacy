import React from 'react';
import { Link } from 'react-router-dom';
import { Row, Col, Container } from 'react-bootstrap';

import strings from './../../constant';
import aboutUsImage from './../../../assets/images/layout/aboutus.png';
import remoteDoctorImage from './../../../assets/images/layout/remote-doctor.png';
import weblogImage from './../../../assets/images/layout/weblog.png';
import fiveEuroImage from './../../../assets/images/layout/5euro.png';

export default class LinksWithInfo extends React.Component {

    render() {
        return (
            <section className='mb-15' id='comp-links-with-info'>
                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className='card padding'>
                                <Row>
                                    <Col xs={12} sm={8}>
                                        <Row>
                                            {[{
                                                title: strings.aboutus,
                                                img: aboutUsImage,
                                                to: '#',
                                                text: 'لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطر آنچنان که لازم است.'
                                            },
                                            {
                                                title: strings.weblog,
                                                img: weblogImage,
                                                to: '#',
                                                text: 'لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطر آنچنان که لازم است.'
                                            },
                                            {
                                                title: strings.remoteDoctor,
                                                img: remoteDoctorImage,
                                                to: '#',
                                                text: 'لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطر آنچنان که لازم است.'
                                            },
                                            {
                                                title: strings.fiveEuroCoupon,
                                                img: fiveEuroImage,
                                                to: '#',
                                                text: 'لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطر آنچنان که لازم است.'
                                            }
                                            ].map((info, idx) =>
                                                <Col key={idx} xs={12} sm={6} className='mb-15'>
                                                    <Link className='link-img d-flex' to={info.to}>
                                                        <img src={info.img} />
                                                        <div className='info'>
                                                            <h5 className='hx'>{info.title}</h5>
                                                            <p className='text-justify'>{info.text}</p>
                                                        </div>
                                                    </Link>
                                                </Col>)}
                                        </Row>
                                    </Col>
                                    <Col xs={12} sm={4} className='direction-column'>
                                        {[{
                                            title: 'دستورالعمل اورال نسخه​',
                                            to: '#',
                                            description: 'روش دستور العمل شا در فیلم'
                                        },
                                        {
                                            title: 'حمل و نقل رایگان​',
                                            to: '#',
                                            description: 'لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است'
                                        },
                                        {
                                            title: 'استخدام دوستان​',
                                            to: '#',
                                            description: 'لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است'
                                        }].map((x, idx) => <Link key={idx} to={x.to} className='link-no-img'>
                                            <h5 className='hx'>{x.title}</h5>
                                            <p>{x.description}</p>
                                        </Link>)}
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