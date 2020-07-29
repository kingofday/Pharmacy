import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import Skeleton from '@material-ui/lab/Skeleton';
import { toast } from 'react-toastify';

import strings from './../../constant';
import srvCategory from './../../../service/srvCategory';

class Categories extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            collapsed: false,
            loading: false,
            items: []
        }
    }
    componentDidMount() {
        document.addEventListener('mousedown', this.handleClickOutside.bind(this));
    }

    componentWillUnmount() {
        document.removeEventListener('mousedown', this.handleClickOutside.bind(this));
    }
    handleClickOutside(event) {
        if (this.wrapper && !this.wrapper.contains(event.target)) {
            this.setState(p => ({ ...p, collapsed: false }));
        }
    }
    async _handleDropDown() {
        if (this.state.collapsed) {
            this.setState(p => ({ ...p, collapsed: false }));
            return;
        }
        this.setState(p => ({ ...p, loading: true, collapsed: true }));
        let get = await srvCategory.get(true);
        this.setState(p => ({ ...p, loading: false }));
        if (!get.success) {
            toast(get.message, { type: toast.TYPE.ERROR });
            return;
        }
        console.log(get);
        this.setState(p => ({ ...p, items: get.result }));
    }
    render() {
        return (
            <section id='comp-menu-categories' ref={c => this.wrapper = c}>
                <button onClick={() => this._handleDropDown()}>
                    <span>{strings.categories}</span>
                    <i className='zmdi zmdi-format-list-bulleted'></i>
                </button>
                <ul className={this.state.collapsed ? 'categories' : 'd-none categories'}>
                    {this.state.loading ? ([0, 1, 2, 3].map(idx => (<li key={idx}><Skeleton height={30} variant='rect' /></li>))) :
                        this.state.items.map((item, idx) => (<li key={idx}>
                            <Link to={`/products?categoryId=${item.categoryId}`}>
                                {item.name}
                            </Link>
                        </li>))
                    }
                </ul>
            </section>

        );
    }
}

// const mapStateToProps = state => {
//     return { ...state.basketReducer, ...state.authenticationReducer };
// }

// const mapDispatchToProps = dispatch => ({
//     logOut: () => dispatch(LogOutAction())
// });

export default connect(null, null)(Categories);