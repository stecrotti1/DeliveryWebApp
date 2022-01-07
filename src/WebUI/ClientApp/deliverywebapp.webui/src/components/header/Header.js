import * as React from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import Toolbar from '@mui/material/Toolbar';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import SearchIcon from '@mui/icons-material/Search';
import Typography from '@mui/material/Typography';
import MuiLink from '@mui/material/Link';

function Header(props) {
    /**
     * @type {{title: string;url: string;}[]}
     */
    const sections = props.sections;

    return (
        <React.Fragment>
            <Toolbar sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <Typography
                    component='h2'
                    variant='h5'
                    color='inherit'
                    align='center'
                    fontFamily={['Roboto', 'sans-serif']}
                    fontWeight='medium'
                    noWrap
                    sx={{ flex: 1 }}
                >
                    DeliveryApp
                </Typography>
                <IconButton>
                    <SearchIcon />
                </IconButton>
                <Button component={Link} to='/signup' variant='outlined' size='small'>
                    Sign up
                </Button>
            </Toolbar>
            <Toolbar
                component='nav'
                variant='dense'
                sx={{ justifyContent: 'space-between', overflowX: 'auto' }}
            >
                {sections.map(section => (
                    <MuiLink
                        color='inherit'
                        noWrap
                        key={section.title}
                        variant='body2'
                        href={section.url}
                        sx={{ p: 1, flexShrink: 0 }}
                    >
                        {section.title}
                    </MuiLink>
                ))}
            </Toolbar>
        </React.Fragment>
    );
}

Header.propTypes = {
    sections: PropTypes.arrayOf(
        PropTypes.shape({
            title: PropTypes.string.isRequired,
            url: PropTypes.string.isRequired
        })
    ).isRequired,
    title: PropTypes.string.isRequired
};

export default Header;