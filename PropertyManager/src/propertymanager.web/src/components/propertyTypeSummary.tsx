import { DataGrid } from '@mui/x-data-grid';
import axios from 'axios';
import React, { useState, useEffect } from "react";
import {
    Box, Button, Container, Stack, SvgIcon, Typography, Card,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
    TableRow } from '@mui/material';
import ArrowDownOnSquareIcon from '@heroicons/react/24/solid/ArrowDownOnSquareIcon';
import ArrowUpOnSquareIcon from '@heroicons/react/24/solid/ArrowUpOnSquareIcon';
import PlusIcon from '@heroicons/react/24/solid/PlusIcon';
import { Scrollbar } from './scrollbar';
import { PropertyType } from '../schemas/propertyTypeSchema';

function PropertyTypeGrid() {

    const [tableData, setTableData] = useState([])

    useEffect(() => {
        (async () => {
                await axios.get('https://localhost:7103/PropertyType').then(function (response)
                {
                    setTableData(response.data);
                }).catch(function (error) {
                    alert(error)
                })                    ;
        })();
    }, []);

    return (

        <Box
            component="main"
            sx={{
                flexGrow: 1,
                py: 8
            }}
        >
            <Container maxWidth="xl">
                <Stack spacing={3}>
                    <Stack
                        direction="row"
                        justifyContent="space-between"
                        spacing={4}
                    >
                        <Stack spacing={1}>
                            <Typography variant="h4">
                                Property Types
                            </Typography>
                            <Stack
                                alignItems="center"
                                direction="row"
                                spacing={1}
                            >
                                <Button
                                    color="inherit"
                                    startIcon={(
                                        <SvgIcon fontSize="small">
                                            <ArrowUpOnSquareIcon />
                                        </SvgIcon>
                                    )}
                                >
                                    Import
                                </Button>
                                <Button
                                    color="inherit"
                                    startIcon={(
                                        <SvgIcon fontSize="small">
                                            <ArrowDownOnSquareIcon />
                                        </SvgIcon>
                                    )}
                                >
                                    Export
                                </Button>
                            </Stack>
                        </Stack>
                        <div>
                            <Button
                                startIcon={(
                                    <SvgIcon fontSize="small">
                                        <PlusIcon />
                                    </SvgIcon>
                                )}
                                variant="contained"
                            >
                                Add
                            </Button>
                        </div>
                    </Stack>
                    {/*<CustomersSearch />*/}
                    <Card>
                        <Scrollbar>
                            <Box sx={{ minWidth: 800 }}>
                                <Table>
                                    <TableHead><TableCell>Name</TableCell></TableHead>
                                    <TableBody>
                                        {tableData.map((propertyType: PropertyType) => {

                                            return (
                                                <TableRow
                                                    hover
                                                    key={propertyType.id}
                                                ><TableCell>{propertyType.name}</TableCell></TableRow>
                                            );
                                        })}
 
                                    </TableBody>
                                </Table>
                                </Box>
                        </Scrollbar>
                    </Card>
                    {/*<CustomersTable*/}
                    {/*    count={data.length}*/}
                    {/*    items={customers}*/}
                    {/*    onDeselectAll={customersSelection.handleDeselectAll}*/}
                    {/*    onDeselectOne={customersSelection.handleDeselectOne}*/}
                    {/*    onPageChange={handlePageChange}*/}
                    {/*    onRowsPerPageChange={handleRowsPerPageChange}*/}
                    {/*    onSelectAll={customersSelection.handleSelectAll}*/}
                    {/*    onSelectOne={customersSelection.handleSelectOne}*/}
                    {/*    page={page}*/}
                    {/*    rowsPerPage={rowsPerPage}*/}
                    {/*    selected={customersSelection.selected}*/}
                    {/*/>*/}
                </Stack>
            </Container>
        </Box>
    );
}

export default PropertyTypeGrid;