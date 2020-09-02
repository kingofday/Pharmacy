import React from 'react';
import { connect } from 'react-redux';
import Heading from './../../../shared/heading/heading';
import { SetNameAction } from './../../../redux/actions/productsAction';
class SearchName extends React.Component {
  state = {
    name: this.props.name||''
  };
  
  _handleChange(e){
    let val = e.target.value;
    this.setState((p) => ({ ...p, name: val }))
  }
  _setName() {
    this.props.setName(this.state.name);
  }
  render() {
    return (
      <div id='comp-search-name' className='card w-100 padding mb-15'>
        <Heading title='جست‌وجو محصول' className='bordered' />
        <div className='form-group'>
          <input name='name' onChange={this._handleChange.bind(this)} value={this.state.name} className='input-name' type='text' placeholder='اینجا جستجو کنید ...' />
          <button onClick={() => this._setName()}>
            <i className='zmdi zmdi-search icon'></i>
          </button>
        </div>
      </div>
    );
  }
}

const mapStateToProps = state => {
  return { ...state.productsReducer };
}

const mapDispatchToProps = dispatch => ({
  setName: (name) => dispatch(SetNameAction(name)),
});


export default connect(mapStateToProps, mapDispatchToProps)(SearchName);