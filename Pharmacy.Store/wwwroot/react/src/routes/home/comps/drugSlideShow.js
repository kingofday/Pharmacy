import React from 'react';
import { connect } from 'react-redux';
import Slider from "react-slick";
import srvDrug from './../../../service/srvDrug';
import Heading from './../../../shared/heading/heading';
import { ShowInitErrorAction } from './../../../redux/actions/InitErrorAction';
import { AddToBasketAction } from './../../../redux/actions/basketAction';
import DrugItem from './../../../shared/DrugItem/drugIten';

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
    this.setState(p => ({ ...p, items: get.result.items, loading: false }));
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
      speed: this.props.speed || 500,
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
          {this.state.loading ? [0, 1, 2, 3, 4].map((x, idx) => (<DrugItem key={idx} loading={true} />)) :
            this.state.items.map((item, idx) => <DrugItem key={idx} item={item} />)
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