// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using osg;

namespace Essence.View.Models
{
    public class OsgDrawing3DModel : AbsDrawing3DModel
    {
        public void Update(Group root)
        {
        }

        public override void Update(IRenderContext3D render)
        {
            if (!this.uptodate)
            {
                this.uptodate = true;
                Group root = (Group)render.ServiceProvider.GetService(typeof(Group));
                this.CreateSampleScene(root);
            }
        }

        private bool uptodate;

        /// <summary>
        /// Crea una escena de ejemplo.
        /// </summary>
        private void CreateSampleScene(Group root)
        {
            //The geode containing our shape
            Geode geode = new Geode();

            ////Our shape: a capsule, it could have been any other geometry (a box, plane, cylinder etc.)
            //Capsule myCapsule = new Capsule(new Vec3f(), 1, 2);

            ////Our shape drawable
            //ShapeDrawable capsuledrawable = new ShapeDrawable(myCapsule);

            //geode.addDrawable(capsuledrawable);

            // create POINTS
            {
                osg.Geometry pointsGeom = new osg.Geometry();
                Vec3Array vertices = new Vec3Array();
                vertices.push_back(new Vec3f(-1.02168f, -2.15188e-09f, 0.885735f));
                vertices.push_back(new Vec3f(-0.976368f, -2.15188e-09f, 0.832179f));
                vertices.push_back(new Vec3f(-0.873376f, 9.18133e-09f, 0.832179f));
                vertices.push_back(new Vec3f(-0.836299f, -2.15188e-09f, 0.885735f));
                vertices.push_back(new Vec3f(-0.790982f, 9.18133e-09f, 0.959889f));
                pointsGeom.setVertexArray(vertices);

                Vec4Array colors = new Vec4Array();
                colors.push_back(new Vec4f(1.0f, 1.0f, 0.0f, 1.0f));

                pointsGeom.setColorArray(colors);
                pointsGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                Vec3Array normals = new Vec3Array();
                normals.push_back(new Vec3f(0.0f, -1.0f, 0.0f));
                pointsGeom.setNormalArray(normals);
                pointsGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                pointsGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.POINTS, 0, (int)vertices.size()));

                geode.addDrawable(pointsGeom);
            }

            // create LINES
            {
                // create Geometry object to store all the vertices and lines primitive.
                osg.Geometry linesGeom = new osg.Geometry();

                // this time we'll preallocate the vertex array to the size we
                // need and then simple set them as array elements, 8 points
                // makes 4 line segments.
                Vec3Array vertices = new Vec3Array(8);
                vertices.set(0, new Vec3f(-1.13704f, -2.15188e-09f, 0.40373f));
                vertices.set(1, new Vec3f(-0.856897f, -2.15188e-09f, 0.531441f));
                vertices.set(2, new Vec3f(-0.889855f, -2.15188e-09f, 0.444927f));
                vertices.set(3, new Vec3f(-0.568518f, -2.15188e-09f, 0.40373f));
                vertices.set(4, new Vec3f(-1.00933f, -2.15188e-09f, 0.370773f));
                vertices.set(5, new Vec3f(-0.716827f, -2.15188e-09f, 0.292498f));
                vertices.set(6, new Vec3f(-1.07936f, 9.18133e-09f, 0.317217f));
                vertices.set(7, new Vec3f(-0.700348f, 9.18133e-09f, 0.362533f));

                // pass the created vertex array to the points geometry object.
                linesGeom.setVertexArray(vertices);

                // set the colors as before, plus using the above
                Vec4Array colors = new Vec4Array();
                colors.push_back(new Vec4f(1.0f, 1.0f, 0.0f, 1.0f));
                linesGeom.setColorArray(colors);
                linesGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // set the normal in the same way color.
                Vec3Array normals = new Vec3Array();
                normals.push_back(new Vec3f(0.0f, -1.0f, 0.0f));
                linesGeom.setNormalArray(normals);
                linesGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // This time we simply use primitive, and hardwire the number of coords to use 
                // since we know up front,
                linesGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.LINES, 0, 8));

                // add the points geometry to the geode.
                geode.addDrawable(linesGeom);
            }

            // create LINE_STRIP
            {
                // create Geometry object to store all the vertices and lines primitive.
                osg.Geometry linesGeom = new osg.Geometry();

                // this time we'll preallocate the vertex array to the size 
                // and then use an iterator to fill in the values, a bit perverse
                // but does demonstrate that we have just a standard std::vector underneath.
                Vec3Array vertices = new Vec3Array();
                vertices.push_back(new Vec3f(-0.0741545f, -2.15188e-09f, 0.416089f));
                vertices.push_back(new Vec3f(0.234823f, -2.15188e-09f, 0.259541f));
                vertices.push_back(new Vec3f(0.164788f, -2.15188e-09f, 0.366653f));
                vertices.push_back(new Vec3f(-0.0288379f, -2.15188e-09f, 0.333695f));
                vertices.push_back(new Vec3f(-0.0453167f, -2.15188e-09f, 0.280139f));

                // pass the created vertex array to the points geometry object.
                linesGeom.setVertexArray(vertices);

                // set the colors as before, plus using the above
                Vec4Array colors = new Vec4Array();
                colors.push_back(new Vec4f(1.0f, 1.0f, 0.0f, 1.0f));
                linesGeom.setColorArray(colors);
                linesGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // set the normal in the same way color.
                Vec3Array normals = new Vec3Array();
                normals.push_back(new Vec3f(0.0f, -1.0f, 0.0f));
                linesGeom.setNormalArray(normals);
                linesGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // This time we simply use primitive, and hardwire the number of coords to use 
                // since we know up front,
                linesGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.LINE_STRIP, 0, 5));

                // add the points geometry to the geode.
                geode.addDrawable(linesGeom);
            }

            // create LINE_LOOP
            {
                // create Geometry object to store all the vertices and lines primitive.
                osg.Geometry linesGeom = new osg.Geometry();

                // this time we'll a C arrays to initialize the vertices.

                Vec3f[] myCoords =
                {
                    new Vec3f(0.741546f, -2.15188e-09f, 0.453167f),
                    new Vec3f(0.840418f, -2.15188e-09f, 0.304858f),
                    new Vec3f(1.12468f, -2.15188e-09f, 0.300738f),
                    new Vec3f(1.03816f, 9.18133e-09f, 0.453167f),
                    new Vec3f(0.968129f, -2.15188e-09f, 0.337815f),
                    new Vec3f(0.869256f, -2.15188e-09f, 0.531441f)
                };

                int numCoords = myCoords.Length;

                Vec3Array vertices = new Vec3Array(new Vec3Vector(myCoords));

                // pass the created vertex array to the points geometry object.
                linesGeom.setVertexArray(vertices);

                // set the colors as before, plus using the above
                Vec4Array colors = new Vec4Array();
                colors.push_back(new Vec4f(1.0f, 1.0f, 0.0f, 1.0f));
                linesGeom.setColorArray(colors);
                linesGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // set the normal in the same way color.
                Vec3Array normals = new Vec3Array();
                normals.push_back(new Vec3f(0.0f, -1.0f, 0.0f));
                linesGeom.setNormalArray(normals);
                linesGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // This time we simply use primitive, and hardwire the number of coords to use 
                // since we know up front,
                linesGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.LINE_LOOP, 0, numCoords));

                // add the points geometry to the geode.
                geode.addDrawable(linesGeom);
            }

            // now we'll stop creating separate normal and color arrays
            // since we are using the same values all the time, we'll just
            // share the same ColorArray and NormalArrays..

            // set the colors as before, use a ref_ptr rather than just
            // standard C pointer, as that in the case of it not being
            // assigned it will still be cleaned up automatically.
            Vec4Array shared_colors = new Vec4Array();
            shared_colors.push_back(new Vec4f(1.0f, 1.0f, 0.0f, 1.0f));

            // same trick for shared normal.
            Vec3Array shared_normals = new Vec3Array();
            shared_normals.push_back(new Vec3f(0.0f, -1.0f, 0.0f));

            // Note on vertex ordering.
            // According to the OpenGL diagram vertices should be specified in a clockwise direction.
            // In reality you need to specify coords for polygons in a anticlockwise direction
            // for their front face to be pointing towards you; get this wrong and you could
            // find back face culling removing the wrong faces of your models.  The OpenGL diagram 
            // is just plain wrong, but it's a nice diagram so we'll keep it for now!

            // create POLYGON
            {
                // create Geometry object to store all the vertices and lines primitive.
                osg.Geometry polyGeom = new osg.Geometry();

                // this time we'll use C arrays to initialize the vertices.
                // note, anticlockwise ordering.
                // note II, OpenGL polygons must be convex, planar polygons, otherwise 
                // undefined results will occur.  If you have concave polygons or ones
                // that cross over themselves then use the osgUtil::Tessellator to fix
                // the polygons into a set of valid polygons.
                Vec3f[] myCoords =
                {
                    new Vec3f(-1.0464f, 0.0f, -0.193626f),
                    new Vec3f(-1.0258f, 0.0f, -0.26778f),
                    new Vec3f(-0.807461f, 0.0f, -0.181267f),
                    new Vec3f(-0.766264f, 0.0f, -0.0576758f),
                    new Vec3f(-0.980488f, 0.0f, -0.094753f)
                };

                int numCoords = myCoords.Length;

                Vec3Array vertices = new Vec3Array(new Vec3Vector(myCoords));

                // pass the created vertex array to the points geometry object.
                polyGeom.setVertexArray(vertices);

                // use the shared color array.
                polyGeom.setColorArray(shared_colors);
                polyGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // use the shared normal array.
                polyGeom.setNormalArray(shared_normals);
                polyGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // This time we simply use primitive, and hardwire the number of coords to use 
                // since we know up front,
                polyGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.POLYGON, 0, numCoords));

                //printTriangles("Polygon", polyGeom);

                // add the points geometry to the geode.
                geode.addDrawable(polyGeom);
            }

            // create QUADS
            {
                // create Geometry object to store all the vertices and lines primitive.
                osg.Geometry polyGeom = new osg.Geometry();

                // note, anticlockwise ordering.
                Vec3f[] myCoords =
                {
                    new Vec3f(0.0247182f, 0.0f, -0.156548f),
                    new Vec3f(0.0247182f, 0.0f, -0.00823939f),
                    new Vec3f(-0.160668f, 0.0f, -0.0453167f),
                    new Vec3f(-0.222464f, 0.0f, -0.13183f),
                    new Vec3f(0.238942f, 0.0f, -0.251302f),
                    new Vec3f(0.333696f, 0.0f, 0.0329576f),
                    new Vec3f(0.164788f, 0.0f, -0.0453167f),
                    new Vec3f(0.13595f, 0.0f, -0.255421f)
                };

                int numCoords = myCoords.Length;

                Vec3Array vertices = new Vec3Array(new Vec3Vector(myCoords));

                // pass the created vertex array to the points geometry object.
                polyGeom.setVertexArray(vertices);

                // use the shared color array.
                polyGeom.setColorArray(shared_colors);
                polyGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // use the shared normal array.
                polyGeom.setNormalArray(shared_normals);
                polyGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // This time we simply use primitive, and hardwire the number of coords to use 
                // since we know up front,
                polyGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.QUADS, 0, numCoords));

                //printTriangles("Quads", *polyGeom);

                // add the points geometry to the geode.
                geode.addDrawable(polyGeom);
            }

            // create QUAD_STRIP
            {
                // create Geometry object to store all the vertices and lines primitive.
                osg.Geometry polyGeom = new osg.Geometry();

                // note, first coord at top, second at bottom, reverse to that buggy OpenGL image..
                Vec3f[] myCoords =
                {
                    new Vec3f(0.733306f, -2.15188e-09f, -0.0741545f),
                    new Vec3f(0.758024f, -2.15188e-09f, -0.205985f),
                    new Vec3f(0.885735f, -2.15188e-09f, -0.0576757f),
                    new Vec3f(0.885735f, -2.15188e-09f, -0.214224f),
                    new Vec3f(0.964009f, 9.18133e-09f, -0.0370773f),
                    new Vec3f(1.0464f, 9.18133e-09f, -0.173027f),
                    new Vec3f(1.11232f, -2.15188e-09f, 0.0123591f),
                    new Vec3f(1.12468f, 9.18133e-09f, -0.164788f),
                };

                int numCoords = myCoords.Length;

                Vec3Array vertices = new Vec3Array(new Vec3Vector(myCoords));

                // pass the created vertex array to the points geometry object.
                polyGeom.setVertexArray(vertices);

                // use the shared color array.
                polyGeom.setColorArray(shared_colors);
                polyGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // use the shared normal array.
                polyGeom.setNormalArray(shared_normals);
                polyGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // This time we simply use primitive, and hardwire the number of coords to use 
                // since we know up front,
                polyGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.QUAD_STRIP, 0, numCoords));

                //printTriangles("Quads strip", *polyGeom);

                // add the points geometry to the geode.
                geode.addDrawable(polyGeom);
            }

            // create TRIANGLES, TRIANGLE_STRIP and TRIANGLE_FAN all in one Geometry/
            {
                // create Geometry object to store all the vertices and lines primitive.
                osg.Geometry polyGeom = new osg.Geometry();

                // note, first coord at top, second at bottom, reverse to that buggy OpenGL image..
                Vec3f[] myCoords =
                {
                    // TRIANGLES 6 vertices, v0..v5
                    // note in anticlockwise order.
                    new Vec3f(-1.12056f, -2.15188e-09f, -0.840418f),
                    new Vec3f(-0.95165f, -2.15188e-09f, -0.840418f),
                    new Vec3f(-1.11644f, 9.18133e-09f, -0.716827f),
                    // note in anticlockwise order.
                    new Vec3f(-0.840418f, 9.18133e-09f, -0.778623f),
                    new Vec3f(-0.622074f, 9.18133e-09f, -0.613835f),
                    new Vec3f(-1.067f, 9.18133e-09f, -0.609715f),
                    // TRIANGLE STRIP 6 vertices, v6..v11
                    // note defined top point first, 
                    // then anticlockwise for the next two points,
                    // then alternating to bottom there after.
                    new Vec3f(-0.160668f, -2.15188e-09f, -0.531441f),
                    new Vec3f(-0.160668f, -2.15188e-09f, -0.749785f),
                    new Vec3f(0.0617955f, 9.18133e-09f, -0.531441f),
                    new Vec3f(0.168908f, -2.15188e-09f, -0.753905f),
                    new Vec3f(0.238942f, -2.15188e-09f, -0.531441f),
                    new Vec3f(0.280139f, -2.15188e-09f, -0.823939f),
                    // TRIANGLE FAN 5 vertices, v12..v16
                    // note defined in anticlockwise order.
                    new Vec3f(0.844538f, 9.18133e-09f, -0.712708f),
                    new Vec3f(1.0258f, 9.18133e-09f, -0.799221f),
                    new Vec3f(1.03816f, -2.15188e-09f, -0.692109f),
                    new Vec3f(0.988727f, 9.18133e-09f, -0.568518f),
                    new Vec3f(0.840418f, -2.15188e-09f, -0.506723f),
                };

                int numCoords = myCoords.Length;

                Vec3Array vertices = new Vec3Array(new Vec3Vector(myCoords));

                // pass the created vertex array to the points geometry object.
                polyGeom.setVertexArray(vertices);

                // use the shared color array.
                polyGeom.setColorArray(shared_colors);
                polyGeom.setColorBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // use the shared normal array.
                polyGeom.setNormalArray(shared_normals);
                polyGeom.setNormalBinding(osg.Geometry.AttributeBinding.BIND_OVERALL);

                // This time we simply use primitive, and hardwire the number of coords to use 
                // since we know up front,
                polyGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.TRIANGLES, 0, 6));
                polyGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.TRIANGLE_STRIP, 6, 6));
                polyGeom.addPrimitiveSet(new DrawArrays((uint)PrimitiveSet.Mode.TRIANGLE_FAN, 12, 5));

                // polygon stipple
                StateSet stateSet = new StateSet();
                polyGeom.setStateSet(stateSet);

//# if !defined(OSG_GLES1_AVAILABLE) && !defined(OSG_GLES2_AVAILABLE) && !defined(OSG_GL3_AVAILABLE)
//                PolygonStipple* polygonStipple = new PolygonStipple;
//                stateSet.setAttributeAndModes(polygonStipple, StateAttribute.Values.OVERRIDE | StateAttribute.Values.ON);
//#endif

                //printTriangles("Triangles/Strip/Fan", *polyGeom);

                // add the points geometry to the geode.
                geode.addDrawable(polyGeom);
            }

            root.addChild(geode);
        }

        private Geode LoadCylinder()
        {
            Cylinder myCylinder = new Cylinder(new Vec3f(0, 0, 0), 0.1f, 1.0f);
            ShapeDrawable myCylinderDrawable = new ShapeDrawable(myCylinder);
            Geode myCylinderGeode = new Geode();
            myCylinderGeode.addDrawable(myCylinderDrawable);
            return myCylinderGeode;
        }

        private Geode LoadBox()
        {
            //create a box
            Box myBox = new Box(new Vec3f(0, 0, 0), 0.5f);
            ShapeDrawable myBoxDrawable = new ShapeDrawable(myBox);
            Geode myBoxGeode = new Geode();
            myBoxGeode.addDrawable(myBoxDrawable);
            //root->addChild(myBoxGeode);
            return myBoxGeode;
        }
    }
}