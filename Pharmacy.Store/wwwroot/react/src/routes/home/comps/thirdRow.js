import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import strings, { enums } from '../../../shared/constant';
import Heading from '../../../shared/heading/heading';
import DrugSlideShow from './drugSlideShow';
import BannerSlideShow from './bannerSlideShow';
import doctorImage from './../../../assets/images/doctor.jpg';

export default class ThirdRow extends React.Component {

  render() {
    return (
      <Row id='comp-third-row' className='comp-third-row mb-15'>
        <Col xs={12} lg={9}>
          <div className='card mb-15'>
            <BannerSlideShow className='d-none d-md-block' />
            <DrugSlideShow title={strings.mostVisited} type={enums.drugFilterType.mostVisited} />
          </div>
        </Col>
        <Col className='description d-none d-lg-flex' lg={3}>
          <div className='card mb-15 padding'>
            <article>
              <p className='text-justify m-b'>
                داروخانه آنلاین بانک محصولات دارویی است. در داروخانه انواع داروها، وسایل آرایشی و بهداشتی عرضه می شود
                در داروخانه همواره یک دکتر داروساز حضور دارد. در صورتی که دارو نیاز به نسخه پزشک داشته باشد، نسخه ی بیمار توسط او چک می شود. تمامی فرآیند آنلاین و حضوری تحویل داروها توسط دکتر داروساز بررسی شده و در مواردی که نیاز است به بیماران در مورد تمامی عوارض و نحوه استفاده از داروها توضیح داده می شود. بنابراین علاوه بر فروشگاه اینترنتی اقلام غیر دارویی، این سایت به عنوان یک مرکز خرید اینترنتی دارو عمل می کند.
              </p>
              <img className='w-100' src={doctorImage} alt='بانک محصولات دارویی' />
            </article>
          </div>
        </Col>
      </Row>
    );
  }
}