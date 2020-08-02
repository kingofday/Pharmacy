import React from 'react';
import {Carousel} from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';

export default class Slider extends React.Component {
    render() {
        return (
            <Carousel>
                {this.props.slides.length === 0 ? (<Skeleton animation='wave' variant='rect' height={320} width={300} />) :
                    (this.props.slides.map((s, idx) => (
                        <Carousel.Item key={idx}>
                            <img className='img-slide' className="d-block" src={s} alt="slide" />
                            <Carousel.Caption>
                                {/* <h3>{s.Title}</h3>
                                <p>{s.Desc}</p> */}
                            </Carousel.Caption>
                        </Carousel.Item>
                    )))}
            </Carousel>
        );
    }
}