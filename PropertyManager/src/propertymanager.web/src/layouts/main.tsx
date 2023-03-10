import Header from '../components/header'
import { Navbar } from '../components/navbar'
import PropertyTypeGrid from '../components/propertyTypeSummary'
import PropertyGrid from '../components/propertySummary'
import { PropertyDetails } from '../components/property'
import Home from '../components/home'
import { BrowserRouter as Router, Routes, Route, Outlet } from "react-router-dom";
import { useEffect, useState } from 'react';
import { styled } from '@mui/system';
import { useTheme } from '@mui/material/styles';

function Main() {
    const theme = useTheme();

    const SIDE_NAV_WIDTH = 280;
    const [openNav, setOpenNav] = useState(true);
    const LayoutRoot = styled('div')(({ theme }) => ({
        display: 'flex',
        flex: '1 1 auto',
        maxWidth: '100%',
        [theme.breakpoints.up('lg')]: {
            paddingLeft: SIDE_NAV_WIDTH
        }
    }));

    const LayoutContainer = styled('div')({
        display: 'flex',
        flex: '1 1 auto',
        flexDirection: 'column',
        width: '100%'
    });

    return (
        
       
            
        <div>
            <Outlet />
            <Header
                //theme={theme}
            ></Header>
                <Navbar
                    onClose={() => setOpenNav(false)}
                open={openNav}
                //theme={theme}
            />
                <LayoutRoot>
                    <LayoutContainer>
                    <Router>
                        <Routes>

                            <Route path="/" element={<Home />} />
                            <Route path="/settings/propertytypes" element={<PropertyTypeGrid />} />
                            <Route path="/properties" element={<PropertyGrid />}>
                                
                            </Route>
                            <Route path="/properties/property/:id" element={<PropertyDetails />} />
                            <Route path="/properties/property/" element={<PropertyDetails />} />
                        </Routes>
                    </Router>   
                    </LayoutContainer>
                </LayoutRoot>
            
                </div>
            
        
                
        //something
            
        //    <div className="content">some more content</div>
        //</div>
    );
}

export default Main