import React, { useState, useEffect } from "react";
import axios from 'axios';
import {
    Checkbox, Box, Button, Container, Stack, SvgIcon, Typography, Card,Link,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
    TableRow,
    Button, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions
} from '@mui/material';
import ArrowDownOnSquareIcon from '@heroicons/react/24/solid/ArrowDownOnSquareIcon';
import ArrowUpOnSquareIcon from '@heroicons/react/24/solid/ArrowUpOnSquareIcon';
import PlusIcon from '@heroicons/react/24/solid/PlusIcon';
import { Scrollbar } from './scrollbar';
import { PropertyInput } from '../schemas/propertySchema';
import { PropertyType } from '../schemas/propertyTypeSchema';
import PencilIcon from '@heroicons/react/24/solid/PencilIcon';
import TrashIcon from '@heroicons/react/24/solid/TrashIcon';
function PropertyGrid() {

    const [tableData, setTableData] = useState([])

    useEffect(() => {
        (async () => {
                await axios.get('https://localhost:7103/Property').then(function (response) {
                    setTableData(response.data);
                }).catch(function (error) {
                    alert(error)
                });

        })();
    }, []);

    function FormatAddress(property) {
        let address = property.addressLine1 + ', ';

        if (property.addressLine2 != '') {
            address += property.addressLine2 + ', ';
        }

        address += property.city += ', ';
        if (property.region != '') {
            address += property.region + ', ';
        }

        address += property.postalCode;
        return address;
    }
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
                                Properties
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
                                component={Link} href="/properties/property"
                            >
                                Add
                            </Button>
                        </div>
                    </Stack>
                    <Card>
                        <Scrollbar>
                            <Box sx={{ minWidth: 800 }}>
                                <Table>
                                    <TableHead><TableCell>&nbsp;</TableCell><TableCell >Address</TableCell><TableCell
                                    //style="width: 20px;"
                                    >Purchase Price</TableCell><TableCell
                                        //style="width: 150px;align: center"
                                        >Purchase Date</TableCell><TableCell>Garage Included?</TableCell></TableHead>
                                    <TableBody>
                                        {tableData.map((property: PropertyInput) => {
                                            var link = "/properties/property/" + property.id?.toString();
                                            return (
                                                <TableRow
                                                    hover
                                                    key={property.id}
                                                ><TableCell><Link href={link}><SvgIcon fontSize="small">
                                                    <PencilIcon />
                                                    </SvgIcon></Link>&nbsp;<SvgIcon fontSize="small">
                                                        <TrashIcon />
                                                    </SvgIcon></TableCell><TableCell >{FormatAddress(property)}</TableCell><TableCell align='right'>{property.purchasePrice}</TableCell><TableCell>{property.purchaseDate?.toString()}</TableCell><TableCell><Checkbox checked={property.garage} /></TableCell></TableRow>
                                            );
                                        })}

                                    </TableBody>
                                </Table>
                            </Box>
                        </Scrollbar>
                    </Card>

                    {/*<CustomersSearch />*/}
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
            <Dialog
                open={this.state.isOpen}
                onClose={this.HandleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title">
                    {this.state.title}
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-dialog-description">
                        Are you sure you wish to delete the selected property?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button onClick={props.deleteFunction} autoFocus>
                        OK
                    </Button>
                </DialogActions>
            </Dialog>)
        </Box>
    );
}

export default PropertyGrid;