import React from 'react';
import Heading from './../heading/heading';
import { Link } from 'react-router-dom';
import srvCategory from './../../service/srvCategory';
import Skeleton from '@material-ui/lab/Skeleton';

export default class Categories extends React.Component {
    state = {
        loading: true,
        categories: []
    }
    async componentDidMount() {
        await this._fetchCategories();
    }

    async _fetchCategories() {
        let srvRep = await srvCategory.get();
        if (srvRep.success) {
            this.setState(p => ({ ...p, categories: srvRep.result }));
        }
    }

    render() {
        return (
            <div className='comp-categories card mb-15'>
                <Heading title='دسته بندی ها' className={this.props.bordered ? 'bordered' : ''} />
                <ul>
                    {this.state.categories.length === 0 ? ([0, 1, 2, 3].map(idx => (<li key={idx}><Skeleton height={30} variant='rect' /></li>))) :
                        this.state.categories.map((item, idx) => (<li key={idx}>
                            <Link to={`/products?categoryId=${item.categoryId}`}>
                                {item.name}
                            </Link>
                        </li>))
                    }
                </ul>
            </div>
        );
    }
}