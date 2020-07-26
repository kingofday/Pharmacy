import React from 'react';
import { connect } from 'react-redux';
import Slider from "react-slick";
import Skeleton from '@material-ui/lab/Skeleton';
import srvDrug from './../../../service/srvDrug';
import { commaThousondSeperator } from './../../../shared/utils';
import strings, { enums } from './../../../shared/constant';
import Heading from './../../../shared/heading/heading';

class LoadingDrug extends React.Component {
  render() {
    return (<figure className='comp-loading-drug'>
      <Skeleton variant='rect' className='main-img' animation="wave" />
      <figcaption className='text-wrapper'>
        <Skeleton variant='text' />
        <Skeleton variant='text' />
      </figcaption>
    </figure>);
  }
}

class DrugSlideShow extends React.Component {
  state = {
    loading: true,
    items: []
  };
  async componentDidMount() {
    var get = await srvDrug.get({ type: enums.drugFilterType.bestSellers });

  }
  render() {
    var settings = {
      dots: false,
      infinite: true,
      speed: 500,
      slidesToShow: 4,
      slidesToScroll: 1,
      dir: 'rtl',
      responsive: [
        {
          breakpoint: 980, // tablet breakpoint
          settings: {
            slidesToShow: 3,
          }
        },
        {
          breakpoint: 576, // mobile breakpoint
          settings: {
            slidesToShow: 1,
          }
        }
      ]
    };
    return (
      <div className='comp-slide-show'>
        <Heading title={strings.bestSellers} className='padding-r'/>
        <Slider {...settings}>
          {this.state.loading ? [0, 1, 2, 3, 4].map((x) => (<LoadingDrug key={x} />)) :
            (this.state.items.map((item, idx) => (<figure key={idx}>
              <img src={item.thumbnailImage} />
              <figcaption>
                <h4>{item.name}</h4>
                <p>{commaThousondSeperator(item.price)} {strings.currency}</p>
              </figcaption>
            </figure>
            )))
          }
        </Slider>
      </div >

    );
  }
}
const mapDispatchToProps = dispatch => ({
});

export default connect(null, mapDispatchToProps)(DrugSlideShow);