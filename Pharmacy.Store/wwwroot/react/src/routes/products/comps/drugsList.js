import React from 'react';
import { connect } from 'react-redux';
import srvDrug from './../../../service/srvDrug';
import { Link } from 'react-router-dom';
import { commaThousondSeperator } from './../../../shared/utils';
import strings, { enums } from '../../../shared/constant';
import Heading from './../../../shared/heading/heading';
import { ShowInitErrorAction } from './../../../redux/actions/InitErrorAction';


class DrugsList extends React.Component {
  state = {
    loading: true,
    items: []
  };
  async _fetchData() {
    var get = await srvDrug.get({ type: enums.drugFilterType.mostVisited.value });
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
    return (
      <div className='comp-drugs-list card w-100 padding mb-15'>
        <Heading title='پرطرفدارها' className='bordered' />
        <ul className='items'>
          {this.state.items.map((item, idx) => <li key={idx} className='item'>
            <Link to={`/product/${item.drugId}`}>
              <img src={item.thumbnailImageUrl} />
              <div className='info'>
                <span>{item.nameFa}</span>
                <strong>{commaThousondSeperator(item.price)} {strings.currency}</strong>
              </div>
            </Link>
          </li>)}
        </ul>
      </div>
    );
  }
}

const mapDispatchToProps = dispatch => ({
  showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message))
});


export default connect(null, mapDispatchToProps)(DrugsList);