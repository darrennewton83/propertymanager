import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemText from '@mui/material/ListItemText';
import Link from '@mui/material/Link';
import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { Scrollbar } from '../components/scrollbar';
import ChevronUpDownIcon from '@heroicons/react/24/solid/ChevronUpDownIcon';
import { Logo } from '../components/logo';
import { SideNavItem } from '../components/side-nav-item';
import HomeModernIcon from '@heroicons/react/24/solid/HomeModernIcon';
import {
    Box,
    Divider,
    Drawer,
    Stack,
    SvgIcon,
    Typography,
} from '@mui/material';
export function Navbar(props) {

    const { open, onClose } = props;
    const drawerWidth = 240;

    const [mobileOpen, setMobileOpen] = useState(true);

    
    const handleDrawerToggle = () => {
        setMobileOpen((prevState) => !prevState);
    };

    const drawer = (
        <Box onClick={handleDrawerToggle} sx={{ textAlign: 'center' }}>
            <List>

                <ListItem disablePadding>
                    <ListItemButton sx={{ textAlign: 'center' }}>
                        <ListItemText></ListItemText>
                    </ListItemButton>
                </ListItem>
                <ListItem disablePadding>
                    <ListItemButton sx={{ textAlign: 'center' }}>
                        <ListItemText><Link href="/settings/propertytypes">Property Types</Link></ListItemText>
                    </ListItemButton>
                </ListItem>
            </List>

            <Link href="/properties">Properties</Link>
        </Box>
    );

    const content = (
        <Scrollbar
            sx={{
                height: '100%',
                '& .simplebar-content': {
                    height: '100%'
                },
                '& .simplebar-scrollbar:before': {
                    background: 'neutral.400'
                }
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    height: '100%'
                }}
            >
                <Box sx={{ p: 3 }}>
                    <Box
                        sx={{
                            display: 'inline-flex',
                            height: 32,
                            width: 32
                        }}
                    >
                        <Logo />
                    </Box>
                    <Box
                        sx={{
                            alignItems: 'center',
                            backgroundColor: 'rgba(255, 255, 255, 0.04)',
                            borderRadius: 1,
                            cursor: 'pointer',
                            display: 'flex',
                            justifyContent: 'space-between',
                            mt: 2,
                            p: '12px'
                        }}
                    >
                        <div>
                            <Typography
                                color="inherit"
                                variant="subtitle1"
                            >
                                Property Manager
                            </Typography>
                            
                        </div>
                        <SvgIcon
                            fontSize="small"
                            sx={{ color: 'neutral.500' }}
                        >
                            <ChevronUpDownIcon />
                        </SvgIcon>
                    </Box>
                </Box>
            </Box>
            <Divider sx={{ borderColor: 'neutral.700' }} />
            <Box
                component="nav"
                sx={{
                    flexGrow: 1,
                    px: 2,
                    py: 3
                }}
            >
                <Stack
                    component="ul"
                    spacing={0.5}
                    sx={{
                        listStyle: 'none',
                        p: 0,
                        m: 0
                    }}
                >
                    <SideNavItem title="Properties" path="/properties" icon={
                        <SvgIcon fontSize="small">
                            <HomeModernIcon />
                        </SvgIcon>
                    } />
                    <SideNavItem title="Property Types" path="/settings/propertytypes" />
                   
                </Stack>
            </Box>
        </Scrollbar>
                );

    return (



        <Drawer
            anchor="left"
            onClose={onClose}
            open={open}
            PaperProps={{
                sx: {
                    backgroundColor: 'neutral.800',
                    color: 'common.white',
                    width: 280
                }
            }}
            sx={{ zIndex: (theme) => theme.zIndex.appBar + 100 }}
            variant="permanent"
        >
            {content }
        </Drawer>

    );
};

Navbar.propTypes = {
    onClose: PropTypes.func,
    open: PropTypes.bool
};                 

