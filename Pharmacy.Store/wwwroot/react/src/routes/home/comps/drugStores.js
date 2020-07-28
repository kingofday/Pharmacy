import React from 'react';
import Slider from "react-slick";
import Skeleton from '@material-ui/lab/Skeleton';
import { Container, Row, Col } from 'react-bootstrap';
import Heading from '../../../shared/heading/heading';
import drugStoreSrv from './../../../service/srvDrugStore';
import strings, { enums } from '../../../shared/constant';

export default class DrugStores extends React.Component {
  state = {
    loading: true,
    items: []
  }
  async _fetchData() {
    let get = await drugStoreSrv.get();
    if (!get.success) {
      return;
    }
    this.setState(p => ({ ...p, items: get.result, loading: false }));
  }
  async componentDidMount() {
    await this._fetchData();
  }
  render() {
    var settings = {
      dots: true,
      infinite: true,
      speed: 500,
      slidesToShow: 5,
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
            slidesToShow: 2,
          }
        }
      ]
    };
    console.log(this.state.items);
    return (
      <Row id='comp-drugstores' className='comp-drugstores mb-15'>
        <Col xs={12} sm={12} className='direction-column'>
          <div class='card padding'>
            <Heading title={strings.drugStores} />
            <Slider {...settings}>
              {this.state.loading ? [0, 1, 2, 3, 4].map((x) => <div key={x}><Skeleton variant='rect' width={100} height={100} /></div>) :
                this.state.items.map((item, idx) => (<div className='drugstore d-flex justify-content-center mb-15' key={idx} title={item.name}>
                  <img src={item.imageUrl} alt={item.name} alt={item.name} />
                </div>))
              }
            </Slider>
          </div>

        </Col>
      </Row>
    );
  }
}