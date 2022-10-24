import { TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Paper } from "@mui/material";
import Vacuna from "./Vacuna";
const TablaVacunas = ({ vacunas }) => {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead sx={{ backgroundColor: "#2E7994" }}>
          <TableRow>
            <TableCell sx={{ color: "white", fontWeight: 600 }}>Tipo vacuna</TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Descripcion
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Tipo pandemia
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Cantidad de dosis
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Reglas
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {vacunas.map((element, index) => (
            <Vacuna vacuna={element} key={index} />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default TablaVacunas;
