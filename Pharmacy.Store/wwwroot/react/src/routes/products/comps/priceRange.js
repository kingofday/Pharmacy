import React from 'react';
import { connect } from 'react-redux';
import Slider from '@material-ui/core/Slider';
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';

import { commaThousondSeperator } from './../../../shared/utils';
import strings from '../../../shared/constant';
import Heading from './../../../shared/heading/heading';
import { SetPriceRangeAction } from './../../../redux/actions/productsAction';

const ltrTheme = createMuiTheme({
  direction: 'ltr',
});

class PriceRange extends React.Component {
  state = {
    max: this.props.maxAvailablePrice,
    range: [this.props.minPrice, this.props.maxPrice]
  };

  _handleChange = (event, newValue) => {
    this.setState(p => ({ ...p, range: newValue }))
  };
  _setPriceRange() {
    console.log(this.state.range[0]);
    this.props.setPriceRange(this.state.range[0], this.state.range[1]);
  }
  // componentWillReceiveProps(nextProps) {
  //   this.setState(p => ({ ...p, max: nextProps.maxPrice, range: [0, nextProps.maxPrice] }));
  // }
  render() {

    return (
      <div id='comp-price-range' className='card w-100 padding mb-15'>
        <Heading title='فیلتر قیمت' className='bordered' />
        <div className='form-group ltr-elm'>
          <MuiThemeProvider theme={ltrTheme}>
            <div dir="ltr">
              <Slider
                track={false}
                min={0}
                max={this.state.max}
                value={this.state.range}
                onChange={this._handleChange.bind(this)}
                aria-labelledby="price-range-slider"
              />
            </div>
          </MuiThemeProvider>
        </div>
        <div className='price-label'>
          <button onClick={this._setPriceRange.bind(this)}>
            اعمال
          </button>
          <span>
            {`${strings.currency} ${commaThousondSeperator(this.state.range[1])}-${commaThousondSeperator(this.state.range[0])}`}
          </span>
        </div>
      </div>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return { ...state.productsReducer };
}

const mapDispatchToProps = dispatch => ({
  setPriceRange: (min, max) => dispatch(SetPriceRangeAction(min, max)),
});


export default connect(mapStateToProps, mapDispatchToProps)(PriceRange);