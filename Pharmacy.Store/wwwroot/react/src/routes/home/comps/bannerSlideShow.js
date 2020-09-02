import React from 'react';
import Slider from "react-slick";
import slide1Image from './../../../assets/images/BannerSlides/slide-1.jpg';
import slide2Image from './../../../assets/images/BannerSlides/slide-2.jpg';

export default class BannerSlideShow extends React.Component {

    render() {
        var settings = {
            dots: false,
            infinite: true,
            speed: 1500,
            slidesToShow: 1,
            slidesToScroll: -1,
            dir: 'rtl',
            arrows: true
        };
        return (
            <div id='comp-banner-slide-show' className={this.props.className || ''}>
                <Slider {...settings}>
                    {[slide1Image, slide2Image].map((image, idx) => <img key={idx} src={image} alt={`banner ${idx}`} />)}
                </Slider>
            </div >

        );
    }
}