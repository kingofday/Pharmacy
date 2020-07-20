import React from 'react';
import { connect } from 'react-redux';
import strings from './../constant';
import ThreeDotLoader from './../threeDotLoader/threeDotLoader';

class SearchItem extends React.Component {
    render() {
        return (
            <li>

            </li>
        );
    }
}
class ActiveItem extends React.Component {
    render() {
        return (
            <div>

            </div>
        );
    }
}
class SearchBar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            items: [],
            activeIndex: 0,
            loading: false,
            showResult: false
        };
    }
    _search() {
        console.log('logging');
        this.setState(p => ({ ...p, loading: true }));
    }
    render() {
        return (
            <div id='comp-search-drug'>
                <div className='input-group'>
                    <input placeholder={strings.searchHere} onInput={this._search.bind(this)} type='search' name='q' />
                    <i className='zmdi zmdi-search'></i>
                    <ThreeDotLoader loading={this.state.loading} />
                </div>
                <ul className={this.state.showResult ? '' : 'd-dnone'}>
                    {
                        this.state.items.map((item) => (<li>
                            item
                        </li>))
                    }
                </ul>
                {this.state.showResult ? <ActiveItem item={this.state.items[this.state.activeIdex]} /> : null}
            </div>
        )
    }
}
// const mapStateToProps = state => {
//     return { ...state.basketReducer, ...state.authenticationReducer };
// }

// const mapDispatchToProps = dispatch => ({
//     logOut: () => dispatch(LogOutAction())
// });

export default connect(null, null)(SearchBar);