﻿/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.PathFinding
{
    /// <summary>
    /// Represents options for the path finder process
    /// </summary>
    public class PathFinderOptions : IPathFinderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathFinderOptions"/> class.
        /// </summary>
        public PathFinderOptions()
        {
            this.usePathSmoothing = true;
            this.maxEscapeCellDistanceIfOriginBlocked = 3;
        }

        /// <summary>
        /// Gets or sets the priority with which this unit's path requests should be processed.
        /// </summary>
        /// <value>
        /// The pathing priority.
        /// </value>
        public int pathingPriority
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the maximum escape cell distance if origin blocked.
        /// This means that when starting a path and the origin (from position) is blocked, this determines how far away the pather will look for a free cell to escape to, before resuming the planned path.
        /// </summary>
        /// <value>
        /// The maximum escape cell distance if origin blocked.
        /// </value>
        public int maxEscapeCellDistanceIfOriginBlocked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether to use path smoothing.
        /// Path smoothing creates more natural routes at a small cost to performance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to path smoothing; otherwise, <c>false</c>.
        /// </value>
        public bool usePathSmoothing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether to allow the path to cut corners. Corner cutting has slightly better performance, but produces less natural routes.
        /// </summary>
        public bool allowCornerCutting
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether diagonal moves are prohibited.
        /// </summary>
        /// <value>
        /// <c>true</c> if diagonal moves are prohibited; otherwise, <c>false</c>.
        /// </value>
        public bool preventDiagonalMoves
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the unit will navigate to the nearest possible position if the actual destination is blocked or otherwise inaccessible.
        /// </summary>
        public bool navigateToNearestIfBlocked
        {
            get;
            set;
        }
    }
}
