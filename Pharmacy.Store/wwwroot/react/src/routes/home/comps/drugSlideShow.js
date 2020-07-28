import React from 'react';
import { connect } from 'react-redux';
import Slider from "react-slick";
import Skeleton from '@material-ui/lab/Skeleton';
import { Link } from 'react-router-dom';
import srvDrug from './../../../service/srvDrug';
import { commaThousondSeperator } from './../../../shared/utils';
import strings from './../../../shared/constant';
import Heading from './../../../shared/heading/heading';
import { ShowInitErrorAction } from './../../../redux/actions/InitErrorAction';
import { AddToBasketAction } from './../../../redux/actions/basketAction';

class LoadingDrug extends React.Component {
  render() {
    return (<figure className='comp-loading-drug'>
      <Skeleton variant='rect' className='main-img' animation="wave" />
      <figcaption className='text-wrapper'>
        <Skeleton className='name' variant='text' />
        <Skeleton className='price' variant='text' />
        <Skeleton className='btn-add' variant='rect' animation="wave" />
      </figcaption>
    </figure>);
  }
}

class DrugSlideShow extends React.Component {
  state = {
    loading: true,
    items: []
  };
  async _fetchData() {
    var get = await srvDrug.get({ type: this.props.type });
    if (!get.success) {
      this.props.showInitError(this._fetchData.bind(this), get.message);
      return;
    }
    this.setState(p => ({ ...p, items: get.result, loading: false }));
  }

  async componentDidMount() {
    await this._fetchData()

  }
  _addToBasket(item) {
    this.props.addToBasket(item, 1);
  }
  render() {
    var settings = {
      autoplay: true,
      dots: false,
      infinite: true,
      speed: this.props.speed||500,
      slidesToShow: 4,
      slidesToScroll: -1,
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
      <section className='comp-slide-show'>
        <Heading title={this.props.title} className='padding-r' />
        <Slider {...settings}>
          {this.state.loading ? [0, 1, 2, 3, 4].map((x) => (<LoadingDrug key={x} />)) :
            (this.state.items.map((item, idx) => (<figure key={idx} className='item'>
              <Link to={`/product/${item.drugId}`} className='d-flex justify-content-center'><img className='main-img' src={item.thumbnailImageUrl} alt={item.nameFa} /></Link>
              <figcaption className='text-center'>
                <h5 className='name'>{item.nameFa}</h5>
                <p className='price'>{commaThousondSeperator(item.price)} {strings.currency}</p>
                <button className='btn-add' onClick={() => this._addToBasket(item)}>
                  {strings.addToCart}
                </button>
              </figcaption>
            </figure>
            )))
          }
        </Slider>
      </section>

    );
  }
}
const mapDispatchToProps = dispatch => ({
  showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
  addToBasket: (item, count) => dispatch(AddToBasketAction(item, count))
});


export default connect(null, mapDispatchToProps)(DrugSlideShow);